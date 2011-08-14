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

        public void ExportTemperatureMin(int year)
        {
			using (log = CreateLogWriter("tmin-missing.log"))
			{
	            for (var grid = GridMin; grid <= GridMax; grid++)
	            {
					var path = CreateDirectory("tmin");
					var file = Path.Combine(path, "gr" + grid.ToString().PadLeft(3, '0'));				
	                using (var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write)) 
					{
						var stations = GetStations(grid, StationDatabase.TemperatureStationtable);
		                var export = new MeasurementExport(stream, stations, year);
						
						var start = new DateTime(year, 1, 1).ToFileTime();
						var end = new DateTime(year, 12, DateTime.DaysInMonth(year, 12)).ToFileTime();
						var query = string.Format(QueryFormat, Td3200Database.TemperatureMinTable, StationDatabase.TemperatureStationtable);
						var measurements = connection.Query<Temperature>(query, new { GridPoint = grid, Start = start, End = end }).ToArray();
		
		               ProcessMeasurements(year, grid, export, measurements);
					}
	            }
			}
        }

        public void ExportTemperatureMax(int year)
        {
			using (log = CreateLogWriter("tmax-missing.log"))
			{
	            for (var grid = GridMin; grid <= GridMax; grid++)
	            {
					var path = CreateDirectory("tmax");                
					var file = Path.Combine(path, "gr" + grid.ToString().PadLeft(3, '0'));				
	                using (var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write)) 
					{
						var stations = GetStations(grid, StationDatabase.TemperatureStationtable);
		                var export = new MeasurementExport(stream, stations, year);
						
						var start = new DateTime(year, 1, 1).ToFileTime();
						var end = new DateTime(year, 12, DateTime.DaysInMonth(year, 12)).ToFileTime();
						var query = string.Format(QueryFormat, Td3200Database.TemperatureMaxTable, StationDatabase.TemperatureStationtable);
						var measurements = connection.Query<Temperature>(query, new { GridPoint = grid, Start = start, End = end }).ToArray();
						
						ProcessMeasurements(year, grid, export, measurements);
					}
	            }
			}
		}
		
		public void ExportPrecipitation(int year) 
		{
			using (log = CreateLogWriter("prcp-missing.log"))
			{
	            for (var grid = GridMin; grid <= GridMax; grid++)
	            {
					var path = CreateDirectory("prcp"); 
					var file = Path.Combine(path, "gr" + grid.ToString().PadLeft(3, '0'));				
	                using (var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write)) 
					{
						var stations = GetStations(grid, StationDatabase.PrecipitationStationTable);
		                var export = new MeasurementExport(stream, stations, year);
						
						var start = new DateTime(year, 1, 1).ToFileTime();
						var end = new DateTime(year, 12, DateTime.DaysInMonth(year, 12)).ToFileTime();
						var query = string.Format(QueryFormat, Td3200Database.PrecipitationTable, StationDatabase.PrecipitationStationTable);
						var measurements = connection.Query<Precipitation>(query, new { GridPoint = grid, Start = start, End = end }).ToArray();
		
		                ProcessMeasurements(year, grid, export, measurements);
					}
	            }
			}
		}
		
		private StreamWriter CreateLogWriter(string file)
		{
			return new StreamWriter(File.Create(Path.Combine(basePath, file)));
		}

		private void ProcessMeasurements(int year, int grid, MeasurementExport export, IMeasurement[] measurements)
		{
			for (var month = 1; month <= 12; month++)
        	{
        		var subset = from measurement in measurements
        			where measurement.Date >= new DateTime(year, month, 1).ToFileTime() &&
        			measurement.Date <= new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToFileTime()
        			select measurement;
        			
        	    export.Export(subset.ToArray(), month);
        	}
        	
			if (export.Missing.Count > 0) 
			{
        		LogMissing(grid, export.Missing);
			}
		}
		
		private void LogMissing(int grid, List<DateTime> missing) 
		{
			log.WriteLine("GridPoint: " + grid);
			
			foreach (DateTime date in missing) 
			{
				log.WriteLine("    Date: " + date.ToShortDateString());
			}
			
			log.Flush();
		}
		
		private string CreateDirectory(string path) 
		{
			var directory = Path.Combine(basePath, path);
			if (!Directory.Exists(directory)) 
			{
				Directory.CreateDirectory(directory);
			}
			
			return directory;
		}
		
		private Station[] GetStations(int grid, string table) 
		{
			var query = "select Id, Sequence, GridPoint from " + table + " where GridPoint = @GridPoint";
            return connection.Query<Station>(query, new { GridPoint = grid }).ToArray();
		}
	}
}