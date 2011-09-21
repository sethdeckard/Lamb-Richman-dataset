using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Mono.Data.Sqlite;
using DataSetExtension;
using DataSetExtension.Database;
using DataSetExtension.Import;
using Dapper;

namespace DataSetExtension.ConsoleApp
{
	public class Commands
	{
		private const string DatabaseName = "DataSetExtension.sqlite";
		
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
		
		public static void ImportPrecipitationStations(string file) 
		{
		
			var count = ImportGridStations(file, GridStationDatabase.PrecipitationStationTable);
			Console.WriteLine(count + " precipitation stations imported");
		}
		
		public static void ImportTemperatureMinStations(string file) 
		{
			var count = ImportGridStations(file, GridStationDatabase.TemperatureMinStationTable);
			Console.WriteLine(count + " temperature min stations imported");
		}
		
		public static void ImportTemperatureMaxStations(string file) 
		{
			var count = ImportGridStations(file, GridStationDatabase.TemperatureMaxStationTable);
			Console.WriteLine(count + " temperature max stations imported");
		}
		
		public static void ImportStations(string file, DateTime start)
		{
			using (IDbConnection connection = CreateConnection())
            {
                connection.Open();

                var database = new StationDatabase(connection);
                database.CreateSchema();
				
				Console.WriteLine("Importing US master station list...");
				
                var import = new StationImport();
                import.Import(new FileStream(file, FileMode.Open, FileAccess.Read), connection);	
				
				Console.WriteLine(import.Total + " master US stations imported");
				
				Console.Write("updating index...");
				database.UpdateIndex();
				Console.WriteLine("done.");
			}			
		}
		
		public static void ImportCanadaStations(string file)
		{
			using (IDbConnection connection = CreateConnection())
            {
                connection.Open();

                var database = new StationDatabase(connection);
                database.CreateSchema();
				
				Console.WriteLine("Importing Canada master station list...");
				
                var import = new CanadaStationImport();
                import.Import(new FileStream(file, FileMode.Open, FileAccess.Read), connection);	
				
				Console.WriteLine(import.Total + " master Canada stations imported");
				
				Console.Write("updating index...");
				database.UpdateIndex();
				Console.WriteLine("done.");
			}			
		}
		
		private static int ImportGridStations(string file, string table) 
		{
			using (IDbConnection connection = CreateConnection())
            {
                connection.Open();

                var database = new GridStationDatabase(connection);
                database.CreateSchema();

                var import = new GridStationImport();
                import.Import(new FileStream(file, FileMode.Open, FileAccess.Read), connection, table);	
				
				Console.Write("updating indices...");
				database.UpdateIndex();
				Console.WriteLine("done.");
				
				return import.Total;
			}	
		}
		
		public static void ImportCanada(string file, int year)
		{
			Console.WriteLine("Importing Canada data...");
			
            using (IDbConnection connection = CreateConnection())
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
				
				Console.Write("updating indices...");
				database.UpdateIndex();
				Console.WriteLine("done.");
				
				Console.WriteLine("Running VACUUM command on database file...");
				connection.Execute("vacuum;");
				Console.WriteLine("Finished.");
            }				
		}
		
		public static void ImportTd3200(string file) 
		{
			Console.WriteLine("Importing TD3200 data...");
			
            using (IDbConnection connection = CreateConnection())
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
				
				Console.Write("updating indices...");
				database.UpdateIndex();
				Console.WriteLine("done.");
				
				Console.WriteLine("Running VACUUM command on database file...");
				connection.Execute("vacuum;");
				Console.WriteLine("Finished.");
            }
		}
		
		public static void Export(string basePath, int year) 
		{
			using (IDbConnection connection = new SqliteConnection(@"Data Source=DataSetExtension.sqlite;Version=3;DateTimeFormat=Ticks"))
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
		
		public static void ExportGridStations(string basePath, int year) 
		{
			using (IDbConnection connection = new SqliteConnection(@"Data Source=DataSetExtension.sqlite;Version=3;DateTimeFormat=Ticks"))
            {
                connection.Open();
				
				var export = new GridStationExport(connection);
				
				ExportTemperatureMinGridStations(export, year, basePath);
				
				ExportTemperatureMaxGridStations(export, year, basePath);
				
				ExportPrecipitationGridStations(export, year, basePath);
			}
		}
		
		private static void ExportTemperatureMinGridStations(GridStationExport export, int year, string basePath)
		{
			Console.Write("Creating tmininfo...");
			
			var file = string.Format("tmininfo-{0}.txt", year);
			using (var stream = File.Create(Path.Combine(basePath, file)))
			{
				var writer = new GridSummaryWriter(stream);
				export.ExportTemperatureMin(writer);
			}
			
			file = string.Format("tmininfo-{0}-historical.txt", year);
			using (var stream = File.Create(Path.Combine(basePath, file)))
			{
				var writer = new GridSummaryWriter(stream) { IncludeHistoricalCount = true };
				export.ExportTemperatureMin(writer);
			}
			
			Console.WriteLine("done.");			
		}
		
		private static void ExportTemperatureMaxGridStations(GridStationExport export, int year, string basePath)
		{
			Console.Write("Creating tmaxinfo...");
	
			var file = string.Format("tmaxinfo-{0}.txt", year);
			using (var stream = File.Create(Path.Combine(basePath, file)))
			{
				var writer = new GridSummaryWriter(stream);
				export.ExportTemperatureMax(writer);
			}
			
			file = string.Format("tmaxinfo-{0}-historical.txt", year);
			using (var stream = File.Create(Path.Combine(basePath, file)))
			{
				var writer = new GridSummaryWriter(stream) { IncludeHistoricalCount = true };
				export.ExportTemperatureMax(writer);
			}
	
			Console.WriteLine("done.");			
		}
		
		private static void ExportPrecipitationGridStations(GridStationExport export, int year, string basePath)
		{
			Console.Write("Creating prcpinfo...");
			
			var file = string.Format("prcpinfo-{0}.txt", year);
			using (var stream = File.Create(Path.Combine(basePath, file)))
			{
				var writer = new GridSummaryWriter(stream);
				export.ExportPrecipitation(writer);
			}
			
			file = string.Format("prcpinfo-{0}-historical.txt", year);
			using (var stream = File.Create(Path.Combine(basePath, file)))
			{
				var writer = new GridSummaryWriter(stream) { IncludeHistoricalCount = true };
				export.ExportPrecipitation(writer);
			}
			
			Console.WriteLine("done.");				
		}
		
		public static void DeleteDatabase() 
		{
			Console.Write("Deleting database...");
			File.Delete(DatabaseName);	
			Console.WriteLine("done.");
		}
		
		public static void CopyDatabase(string path, string name)
		{
			Console.Write("Copying database to " + path);
			File.Copy(DatabaseName, Path.Combine(path, name), true);	
			Console.WriteLine(" done.");
		}
	}
}