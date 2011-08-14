using System;
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
		private const string QueryFormat = "select StationId, Date, Value from {0} inner join {1} on {1}.Id = {0}.StationId where GridPoint = @GridPoint and Date >= @Start and Date <= @End";

        private readonly string basePath;
        private readonly IDbConnection connection;

        public ExportController(IDbConnection connection, string path)
        {
            this.connection = connection;
            basePath = path;
        }

        public void ExportTemperatureMin(int year)
        {
            for (var grid = GridMin; grid <= GridMax; grid++)
            {
                var path = Path.Combine(basePath, "tmin");
				if (!Directory.Exists(path)) 
				{
					Directory.CreateDirectory(path);
				}
				
				var file = Path.Combine(path, "gr" + grid.ToString().PadLeft(3, '0'));				
                using (var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write)) 
				{
					var stations = GetStations(grid, StationDatabase.TemperatureStationtable);
	                var export = new MeasurementExport(stream, stations, year);
	
	                for (var month = 1; month <= 12; month++)
	                {
	                    var query = string.Format(QueryFormat, Td3200Database.TemperatureMinTable, StationDatabase.TemperatureStationtable);
						
						var start = new DateTime(year, month, 1).ToFileTime();
						var end = new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToFileTime();
						
	                    var records = connection.Query<Temperature>(query, new { GridPoint = grid, Start = start, End = end }).ToArray();
	                    export.Export(records, month);
	                }
				}
            }
        }

        public void ExportTemperatureMax(int year)
        {
            for (var grid = GridMin; grid <= GridMax; grid++)
            {
                var path = Path.Combine(basePath, "tmax");
				if (!Directory.Exists(path)) 
				{
					Directory.CreateDirectory(path);
				}
				
				var file = Path.Combine(path, "gr" + grid.ToString().PadLeft(3, '0'));				
                using (var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write)) 
				{
					var stations = GetStations(grid, StationDatabase.TemperatureStationtable);
	                var export = new MeasurementExport(stream, stations, year);
	
	                for (var month = 1; month <= 12; month++)
	                {
	                    var query = string.Format(QueryFormat, Td3200Database.TemperatureMaxTable, StationDatabase.TemperatureStationtable);
						
						var start = new DateTime(year, month, 1).ToFileTime();
						var end = new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToFileTime();
						
	                    var records = connection.Query<Temperature>(query, new { GridPoint = grid, Start = start, End = end }).ToArray();
	                    export.Export(records, month);
	                }
				}
            }			
		}
		
		public void ExportPrecipitation(int year) 
		{
            for (var grid = GridMin; grid <= GridMax; grid++)
            {
                var path = Path.Combine(basePath, "prcp");
				if (!Directory.Exists(path)) 
				{
					Directory.CreateDirectory(path);
				}
				
				var file = Path.Combine(path, "gr" + grid.ToString().PadLeft(3, '0'));				
                using (var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write)) 
				{
					var stations = GetStations(grid, StationDatabase.PrecipitationStationTable);
	                var export = new MeasurementExport(stream, stations, year);
	
	                for (var month = 1; month <= 12; month++)
	                {
	                    var query = string.Format(QueryFormat, Td3200Database.PrecipitationTable, StationDatabase.PrecipitationStationTable);
						
						var start = new DateTime(year, month, 1).ToFileTime();
						var end = new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToFileTime();
						
	                    var records = connection.Query<Precipitation>(query, new { GridPoint = grid, Start = start, End = end }).ToArray();
	                    export.Export(records, month);
	                }
				}
            }			
		}
		
		private Station[] GetStations(int grid, string table) 
		{
			var query = "select Id, Sequence, GridPoint from " + table + " where GridPoint = @GridPoint";
            return connection.Query<Station>(query, new { GridPoint = grid }).ToArray();
		}
	}
}

