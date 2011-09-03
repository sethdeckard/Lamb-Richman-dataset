using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Dapper;

namespace DataSetExtension
{
	public class ExportController
	{
        private const int GridMin = 1;
        private const int GridMax = 766;
		private const string QueryFormat = "select StationId, Date, Value from {0} inner join {1} on {1}.Id = {0}.StationId " + 
			"where GridPoint = @GridPoint and Date >= @Start and Date <= @End";

        private readonly string basePath;
        private readonly IDbConnection connection;
		private StreamWriter log;

        public ExportController(IDbConnection connection, string path)
        {
            this.connection = connection;
            basePath = path;
        }
		
		public IMeasurementLocator Locator { get; set; }
		
        public void ExportTemperatureMin(int year)
        {
			using (log = CreateLogWriter(string.Format("tmin-missing-{0}.log", year)))
			{
	            for (var grid = GridMin; grid <= GridMax; grid++)
	            {			
	                using (var stream = new FileStream(GetFile(grid, "tmin"), FileMode.OpenOrCreate, FileAccess.Write)) 
					{
						stream.Seek(0, SeekOrigin.End);
						
						var stations = GetStations(grid, GridStationDatabase.TemperatureMinStationTable);
						var locator = new MeasurementLocator(connection, MeasurementDatabase.TemperatureMinTable);
		                var export = new MeasurementWriter(stream, stations, year) { Locator = locator, Formatter = new TemperatureFormatter() };
						
						var start = new DateTime(year, 1, 1);
						var end = GetEndDate(year);
						var query = string.Format(QueryFormat, MeasurementDatabase.TemperatureMinTable, GridStationDatabase.TemperatureMinStationTable);
						var measurements = connection.Query<Measurement>(query, new { GridPoint = grid, Start = start, End = end }).ToArray();
		
		               ProcessMeasurements(year, grid, export, measurements);
					}
	            }
			}
        }

        public void ExportTemperatureMax(int year)
        {
			using (log = CreateLogWriter(string.Format("tmax-missing-{0}.log", year)))
			{
	            for (var grid = GridMin; grid <= GridMax; grid++)
	            {			
	                using (var stream = new FileStream(GetFile(grid, "tmax"), FileMode.OpenOrCreate, FileAccess.Write)) 
					{
						stream.Seek(0, SeekOrigin.End);
						
						var stations = GetStations(grid, GridStationDatabase.TemperatureMaxStationTable);
						var locator = new MeasurementLocator(connection, MeasurementDatabase.TemperatureMaxTable);
		                var export = new MeasurementWriter(stream, stations, year) { Locator = locator, Formatter = new TemperatureFormatter() };
						
						var start = new DateTime(year, 1, 1);
						var end = GetEndDate(year);
						var query = string.Format(QueryFormat, MeasurementDatabase.TemperatureMaxTable, GridStationDatabase.TemperatureMaxStationTable);
						var measurements = connection.Query<Measurement>(query, new { GridPoint = grid, Start = start, End = end }).ToArray();
						
						ProcessMeasurements(year, grid, export, measurements);
					}
	            }
			}
		}
		
		public void ExportPrecipitation(int year) 
		{
			using (var addedLog = CreateLogWriter(string.Format("prcp-added-{0}.log", year)))
			{	
				using (log = CreateLogWriter(string.Format("prcp-missing-{0}.log", year)))
				{
					var tracker = new StationTracker();
					var locator = new MeasurementLocator(connection, MeasurementDatabase.PrecipitationTable)
						{
							Tracker = tracker
						};
					
					var formatter = new PrecipitationFormatter();
					
		            for (var grid = GridMin; grid <= GridMax; grid++)
		            {
		                using (var stream = new FileStream(GetFile(grid, "prcp"), FileMode.OpenOrCreate, FileAccess.Write)) 
						{
							stream.Seek(0, SeekOrigin.End);
							
							var stations = GetStations(grid, GridStationDatabase.PrecipitationStationTable);
							//might need a LoadStations method here on Locator
			                var export = new MeasurementWriter(stream, stations, year) { Locator = locator, Formatter = formatter };
							
							var start = new DateTime(year, 1, 1);
							var end = GetEndDate(year);
							var query = string.Format(QueryFormat, MeasurementDatabase.PrecipitationTable, GridStationDatabase.PrecipitationStationTable);
							var measurements = connection.Query<Measurement>(query, new { GridPoint = grid, Start = start, End = end }).ToArray();
			
			                ProcessMeasurements(year, grid, export, measurements);
							
							UpdateStations(export.GetUpdatedStations(), GridStationDatabase.PrecipitationStationTable, addedLog);
						}
		            }
				}		
			}
		}
								
		private void UpdateStations(GridStation[] stations, string table, StreamWriter log)
		{
			foreach (var station in stations)
			{
				if (station.IsNew) 
				{
					log.WriteLine(station.Number);
					continue;
				}
	
				
				//update station properties
			}
		}
		
		private string GetFile(int grid, string directory)
		{ 
			return Path.Combine(GetDirectory(directory), "gr" + grid.ToString().PadLeft(3, '0'));
		}
		
		private StreamWriter CreateLogWriter(string file)
		{
			return new StreamWriter(File.Create(Path.Combine(basePath, file)));
		}

		private void ProcessMeasurements(int year, int grid, MeasurementWriter export, IMeasurement[] measurements)
		{
			for (var month = 1; month <= 12; month++)
        	{
        		var subset = from measurement in measurements
        			where measurement.Date >= new DateTime(year, month, 1) &&
        			measurement.Date <= new DateTime(year, month, DateTime.DaysInMonth(year, month))
        			select measurement;
        			
        	    export.Write(subset.ToArray(), month);
        	}
        	
			if (export.Missing.Count > 0) 
			{
        		LogMissing(grid, export.Missing);
			}
		}
		
		private void LogMissing(int grid, List<DateTime> missing) 
		{
			log.WriteLine(grid);
			
			foreach (DateTime date in missing) 
			{
				log.WriteLine("    " + date.ToShortDateString());
			}
			
			log.Flush();
		}
		
		private string GetDirectory(string path) 
		{
			var directory = Path.Combine(basePath, path);
			if (!Directory.Exists(directory)) 
			{
				Directory.CreateDirectory(directory);
			}
			
			return directory;
		}
		
		private GridStation[] GetStations(int grid, string table) 
		{
			var query = "select Id, Sequence, GridPoint from " + table + " where GridPoint = @GridPoint";
            return connection.Query<GridStation>(query, new { GridPoint = grid }).ToArray();
		}

		private DateTime GetEndDate (int year)
		{
			return new DateTime(year, 12, DateTime.DaysInMonth(year, 12));
		}
	}
}