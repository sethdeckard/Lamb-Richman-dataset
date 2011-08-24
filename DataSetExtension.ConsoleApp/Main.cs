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
			ImportTemperatureMinStations(@"/Users/seth/Documents/LRDataSet/tmininfo.txt");
			ImportTemperatureMaxStations(@"/Users/seth/Documents/LRDataSet/tmaxinfo.txt");*/
			
			//ImportTd3200(@"/Users/seth/Documents/LRDataSet/data/TimeSeries_2010.txt", 0);
			//Export(@"/Users/seth/Documents/LRDataSet/output", 2009);
			//Export(@"/Users/seth/Documents/LRDataSet/output", 2010);
		}
		
		private static void ImportPrecipitationStations(string file) 
		{
			var count = ImportStations(file, GridStationDatabase.PrecipitationStationTable);
			Console.WriteLine(count + " precipitation stations imported");
		}
		
		private static void ImportTemperatureMinStations(string file) 
		{
			var count = ImportStations(file, GridStationDatabase.TemperatureMinStationTable);
			Console.WriteLine(count + " temperature min stations imported");
		}
		
		private static void ImportTemperatureMaxStations(string file) 
		{
			var count = ImportStations(file, GridStationDatabase.TemperatureMaxStationTable);
			Console.WriteLine(count + " temperature max stations imported");
		}
		
		private static int ImportStations(string file, string table) 
		{
			using (IDbConnection connection = new SqliteConnection(@"Data Source=DataSetExtension.sqlite;Version=3;Journal Mode=Off;Synchronous=Off"))
            {
                connection.Open();

                var database = new GridStationDatabase(connection);
                database.CreateSchema();

                var import = new GridStationImport();
                import.Import(new FileStream(file, FileMode.Open, FileAccess.Read), connection, table);	
				
				return import.Imported.Count;
			}	
		}
		
		private static void ImportTd3200(string file) 
		{
			Console.WriteLine("Importing TD3200 data...");
			
            using (IDbConnection connection = new SqliteConnection(@"Data Source=DataSetExtension.sqlite;Version=3;Journal Mode=Off;Synchronous=Off"))
            {
                connection.Open();
				
				var tempMinStations = connection.Query<GridStation>("select Id, Number, GridPoint from " + GridStationDatabase.TemperatureMinStationTable).ToArray();
				
				var tempMaxStations = connection.Query<GridStation>("select Id, Number, GridPoint from " + GridStationDatabase.TemperatureMaxStationTable).ToArray();
				
				var precipStations = connection.Query<GridStation>("select Id, Number, GridPoint from " + GridStationDatabase.PrecipitationStationTable).ToArray();
				
                var import = new Td3200Import 
					{
						TemperatureMinStations = tempMinStations, 
						TemperatureMaxStations = tempMaxStations, 
						PrecipitationStations = precipStations
					};

                var database = new MeasurementDatabase(connection);
                database.CreateSchema();

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                import.Import(new FileStream(file, FileMode.Open, FileAccess.Read), connection);
				
				stopwatch.Stop();
				
				Console.WriteLine(import.Total + " TD3200 records imported.");
				Console.WriteLine("Total TD3200 Import time: " + stopwatch.Elapsed.ToString());
				
				Console.WriteLine("Running VACUUM command on database file...");
				connection.Execute("vacuum;");
				Console.WriteLine("Finished.");
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