using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Mono.Data.Sqlite;
using DataSetExtension;
using DataSetExtension.Import;
using Dapper;

namespace DataSetExtension.ConsoleApp
{
	class MainClass
	{
		private const string DatabaseName = "DataSetExtension.sqlite";
		
		public static void Main(string[] args)
		{
			ImportPrecipitationStations(@"/Users/seth/Documents/LRDataSet/prcpinfo.txt");
			ImportTemperatureMinStations(@"/Users/seth/Documents/LRDataSet/tmininfo.txt");
			ImportTemperatureMaxStations(@"/Users/seth/Documents/LRDataSet/tmaxinfo.txt");
			
			ImportTd3200(@"/Users/seth/Documents/LRDataSet/data/TimeSeries_2001.txt");
			
			//ImportCanada(@"/Users/seth/Documents/LRDataSet/canada-data/canada.all");
			
			//Export(@"/Users/seth/Documents/LRDataSet/output", 2001);
			//Export(@"/Users/seth/Documents/LRDataSet/output", 2010);
			
			//ImportStations(@"/Users/seth/Documents/LRDataSet/COOP.TXT.2007may", new DateTime(2001, 1, 1));
		}
		
		private static IDbConnection CreateConnection() 
		{
			var builder = new SqliteConnectionStringBuilder
			{
				DataSource = DatabaseName,
				Version = 3,
				JournalMode = SQLiteJournalModeEnum.Off,
				SyncMode = SynchronizationModes.Off,
				DateTimeFormat = SQLiteDateFormats.Ticks				
			};
			
			return new SqliteConnection(builder.ToString());
		}
		
		private static void ImportPrecipitationStations(string file) 
		{
			var count = ImportGridStations(file, GridStationDatabase.PrecipitationStationTable);
			Console.WriteLine(count + " precipitation stations imported");
		}
		
		private static void ImportTemperatureMinStations(string file) 
		{
			var count = ImportGridStations(file, GridStationDatabase.TemperatureMinStationTable);
			Console.WriteLine(count + " temperature min stations imported");
		}
		
		private static void ImportTemperatureMaxStations(string file) 
		{
			var count = ImportGridStations(file, GridStationDatabase.TemperatureMaxStationTable);
			Console.WriteLine(count + " temperature max stations imported");
		}
		
		private static void ImportStations(string file, DateTime start)
		{
			using (IDbConnection connection = CreateConnection())
            {
                connection.Open();

                var database = new StationDatabase(connection);
                database.CreateSchema();

                var import = new StationImport();
                import.Import(new FileStream(file, FileMode.Open, FileAccess.Read), connection);	
				
				Console.WriteLine(import.Total + " master stations imported");
			}			
		}
		
		private static int ImportGridStations(string file, string table) 
		{
			using (IDbConnection connection = new SqliteConnection(@"Data Source=DataSetExtension.sqlite;Version=3;Journal Mode=Off"))
            {
                connection.Open();

                var database = new GridStationDatabase(connection);
                database.CreateSchema();

                var import = new GridStationImport();
                import.Import(new FileStream(file, FileMode.Open, FileAccess.Read), connection, table);	
				
				return import.Imported.Count;
			}	
		}
		
		private static void ImportCanada(string file, int year)
		{
			Console.WriteLine("Importing Canada data...");
			
            using (IDbConnection connection = new SqliteConnection(@"Data Source=DataSetExtension.sqlite;Version=3;Journal Mode=Off;Synchronous=Off"))
            {
                connection.Open();
				
				var tempMinStations = connection.Query<GridStation>("select Id, Number, GridPoint from " + GridStationDatabase.TemperatureMinStationTable).ToArray();
				
				var tempMaxStations = connection.Query<GridStation>("select Id, Number, GridPoint from " + GridStationDatabase.TemperatureMaxStationTable).ToArray();
				
				var precipStations = connection.Query<GridStation>("select Id, Number, GridPoint from " + GridStationDatabase.PrecipitationStationTable).ToArray();
				
                var import = new CanadaImport 
					{
						TemperatureMinStations = tempMinStations, 
						TemperatureMaxStations = tempMaxStations, 
						PrecipitationStations = precipStations,
						Year = year
					};

                var database = new MeasurementDatabase(connection);
                database.CreateSchema();

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                import.Import(new FileStream(file, FileMode.Open, FileAccess.Read), connection);
				
				stopwatch.Stop();
				
				Console.WriteLine(import.Total + " Canada records imported.");
				Console.WriteLine("Total Canada Import time: " + stopwatch.Elapsed.ToString());
				
				Console.WriteLine("Running VACUUM command on database file...");
				connection.Execute("vacuum;");
				Console.WriteLine("Finished.");
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