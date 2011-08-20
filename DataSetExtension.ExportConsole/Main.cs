using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Mono.Data.Sqlite;
using DataSetExtension;
using Dapper;

namespace DataSetExtension.ExportConsole
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
 
            var year = GetYear(args);
            if (year == 0 && command != "help")
            {
                Console.Error.WriteLine("Missing or invalid value provided for year.");
                command = "help";
            }
 
            var directory = GetDirectory(args);
            ProcessCommand(command, year, directory);
        }
 
        private static int GetYear(IList<string> args)
        {
            var year = 0;
            if (args.Count > 1 && !string.IsNullOrEmpty(args[1]))
            {
                int.TryParse(args[1], out year);
            }
 
            return year;
        }
 
        private static string GetDirectory(IList<string> args)
        {
            string directory;
            if (args.Count > 2 && !string.IsNullOrEmpty(args[2]))
            {
                directory = args[2];
            }
            else
            {
                directory = Directory.GetCurrentDirectory();
            }
 
            return directory;
        }
 
        private static void ProcessCommand(string command, int year, string directory)
        {
            if (command == "tmin")
            {
                Console.WriteLine(year);
                Console.WriteLine(directory);
            }
            else if (command == "tmax")
            {
 
            }
            else if (command == "prcp")
            {
            }
            else if (command == "all")
            {
            }
            else
            {
                ShowHelp();
            }
        }
 
        private static void ShowHelp()
        {
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("=================================== Export ====================================");
            Console.WriteLine("Usage:");
            Console.WriteLine("    export type year [directory]");
            Console.WriteLine("Example:");
            Console.WriteLine("    export tmin 2011 /users/bob/output");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("Options:");
            Console.WriteLine("    type: Type of data, valid options are: tmin, tmax, prcp, all");
            Console.WriteLine("    year: 4 digit year");
            Console.WriteLine("    directory: [optional] Directory for output, default is current directory");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("Notes:");
            Console.WriteLine("    This program requires a SQLite database named DataSetExtension.sqlite, this");
            Console.WriteLine("database should already be populated with stations and raw data for the year");
            Console.WriteLine("attempting to be exported, run the import command to perform these tasks first.");
            Console.WriteLine("-------------------------------------------------------------------------------");
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
