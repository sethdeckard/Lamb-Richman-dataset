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
		
		public static void Main(string[] args)
		{
			/*ImportPrecipitationStations(@"/Users/seth/Documents/LRDataSet/prcpinfo.txt");
			ImportTemperatureStations(@"/Users/seth/Documents/LRDataSet/tmaxinfo.txt");
			ImportTd3200(@"/Users/seth/Documents/LRDataSet/data/TimeSeries_2001.txt", 2001);*/
			
			Export(@"/Users/seth/Documents/LRDataSet/output", 2001);
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
				
				Console.WriteLine(import.Total + " TD3200 records imported.");
				Console.WriteLine("Total TD3200 Import time: " + stopwatch.Elapsed.ToString());
            }
		}
		
		private static void Export(string basePath, int year) 
		{
			using (IDbConnection connection = new SqliteConnection(@"Data Source=DataSetExtension.sqlite;Version=3"))
            {
                connection.Open();
				
				var controller = new ExportController(connection, basePath);
				
				Console.WriteLine("Exporting TemperatureMin...");
				var stopwatch = new Stopwatch();
                stopwatch.Start();
				controller.ExportTemperatureMin(year);
				stopwatch.Stop();
				Console.WriteLine("Total TemperatureMin export time: " + stopwatch.Elapsed.ToString());
				
				Console.WriteLine("Exporting TemperatureMax...");
				stopwatch = new Stopwatch();
                stopwatch.Start();
				controller.ExportTemperatureMax(year);
				stopwatch.Stop();
				Console.WriteLine("Total TemperatureMax export time: " + stopwatch.Elapsed.ToString());
				
				Console.WriteLine("Exporting Precipitation...");
				stopwatch = new Stopwatch();
                stopwatch.Start();
				controller.ExportPrecipitation(year);
				stopwatch.Stop();
				Console.WriteLine("Total Precipitation export time: " + stopwatch.Elapsed.ToString());
			}
		}
	}
}