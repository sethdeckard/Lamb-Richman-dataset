using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Mono.Data.Sqlite;
using DataSetExtension;
using DataSetExtension.Import;
using Dapper;

namespace DataSetExtension.ImportConsole
{
	class MainClass
	{
        static void Main(string[] args)
        {
            var command = "help";
            if (args.Length > 0)
            {
                command = args[0];
            }
 
            var file = GetFile(args);
            if (file == null)
            {
                Console.Error.WriteLine("File name not provided");
                command = "help";
            }
 
            ProcessCommand(command, file);
        }
 
        private static string GetFile(IList<string> args)
        {
            string file = null;
            if (args.Count > 1 && !string.IsNullOrEmpty(args[1]))
            {
                file = args[1];
            }
 
            return file;
        }
 
        private static void ProcessCommand(string command, string file)
        {
            if (command == "td3200")
            {
                ImportTd3200(file);
            }
            else if (command == "ca")
            {
				throw new NotImplementedException("Canadian import is not implemented.");
            }
            else if (command == "tmininfo")
            {
				ImportTemperatureMinStations(file);
            }
            else if (command == "tmaxinfo")
            {
				ImportTemperatureMaxStations(file);
            }
            else if (command == "prcpinfo")
            {
				ImportTemperatureMinStations(file);
            }
            else
            {
                ShowHelp();
            }
        }
 
        private static void ShowHelp()
        {
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("=================================== Import ====================================");
            Console.WriteLine("Usage:");
            Console.WriteLine("    import type file");
            Console.WriteLine("Example:");
            Console.WriteLine("    import td3200 /users/bob/TimeSeries-2011.txt");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("Options:");
            Console.WriteLine("    type: Type of data, valid options are:");
            Console.WriteLine("        td3200 - TD3200 format US COOP station data, plain text.");
            Console.WriteLine("        ca - Canadian raw data, delimited format.");
            Console.WriteLine("        tmininfo - File containing tmin grid points, stations, and record ");
            Console.WriteLine("                   counts generated from a previous export.");
            Console.WriteLine("        tmaxinfo - File containing tmax grid points, stations, and record ");
            Console.WriteLine("                   counts generated from a previous export.");
            Console.WriteLine("        prcpinfo - File containing prcp grid points, stations, and record ");
            Console.WriteLine("                   counts generated from a previous export.");
            Console.WriteLine("    file: Qualified path and file name of the source file to import.");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("Notes:");
            Console.WriteLine("    Stations (*info.txt) must be imported before TD3200 and Canadian data is");
            Console.WriteLine("imported into the system. This program will create a SQLite database named ");
            Console.WriteLine("DataSetExtension.sqlite if it doesn't already exist. You can use an existing");
            Console.WriteLine("database populated with stations to skip the station *info import steps.");
            Console.WriteLine("-------------------------------------------------------------------------------");
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
	}
}