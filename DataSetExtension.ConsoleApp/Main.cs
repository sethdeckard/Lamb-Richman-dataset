using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Mono.Data.Sqlite;
using DataSetExtension;
using Dapper;

namespace DataSetExtension.ConsoleApp
{
	class MainClass
	{
		private const string DatabaseName = "DataSetExtension.sqlite";
		
		public static void Main (string[] args)
		{
			ImportPrecipitationStations(@"/Users/seth/Documents/LRDataSet/prcpinfo.txt");
			ImportTemperatureStations(@"/Users/seth/Documents/LRDataSet/tmaxinfo.txt");
			
			ImportTd3200(@"/Users/seth/Documents/LRDataSet/co05stn.dat", 2001);
		}
		
		private static void ImportPrecipitationStations(string file) 
		{
			using (IDbConnection connection = new SqliteConnection(@"Data Source=DataSetExtension.sqlite;Version=3"))
            {
                connection.Open();

                var database = new StationDatabase(connection);
                database.CreateSchema();

                var import = new StationImport();
                import.Import(new FileStream(file, FileMode.Open, FileAccess.Read), connection, StationDatabase.PrecipitationStationTable);
				
				Console.WriteLine(import.Imported.Count + " precipitation stations imported");
			}
		}
		
		private static void ImportTemperatureStations(string file) 
		{
			using (IDbConnection connection = new SqliteConnection(@"Data Source=DataSetExtension.sqlite;Version=3"))
            {
                connection.Open();

                var database = new StationDatabase(connection);
                database.CreateSchema();

                var import = new StationImport();
                import.Import(new FileStream(file, FileMode.Open, FileAccess.Read), connection, StationDatabase.TemperatureStationtable);	
				
				Console.WriteLine(import.Imported.Count + " temperature stations imported");
			}
		}
		
		private static void ImportTd3200(string file, int year) 
		{
            using (IDbConnection connection = new SqliteConnection(@"Data Source=DataSetExtension.sqlite;Version=3"))
            {
                connection.Open();
				
				var tempStations = connection.Query<Station>("select Id, Number, GridPoint from " + StationDatabase.TemperatureStationtable).ToArray();
				
				var percipStations = connection.Query<Station>("select Id, Number, GridPoint from " + StationDatabase.PrecipitationStationTable).ToArray();
				
                var import = new Td3200Import(tempStations.ToArray(), percipStations.ToArray()) { Year = year };

                var database = new Td3200Database(connection);
                database.CreateSchema();

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                import.Import(new FileStream(file, FileMode.Open, FileAccess.Read), connection);
				
				stopwatch.Stop();
            }
		}
	}
}