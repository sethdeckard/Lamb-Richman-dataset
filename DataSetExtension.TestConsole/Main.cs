using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Diagnostics;
using System.IO;
using DataSetExtension;

namespace DataSetExtension.TestConsole
{
	class MainClass
	{
        static void Main(string[] args)
        {
            ImportStations();
        }

        private static void ImportStations()
        {
            using (IDbConnection connection = new SqliteConnection(@"Data Source=/Users/seth/Documents/LRDataSet/DataSetExtension.sqlite;Version=3"))
            {
                connection.Open();

                var database = new StationDatabase(connection);
                database.CreateTables();

                var import = new StationImport();
                import.Import(new FileStream(@"/Users/seth/Documents/LRDataSet/prcpinfo.txt", FileMode.Open, FileAccess.Read), connection, StationDatabase.PrecipitationStationTable);

                var percipStations = import.Imported;

                import = new StationImport();
                import.Import(new FileStream(@"/Users/seth/Documents/LRDataSet/tmaxinfo.txt", FileMode.Open, FileAccess.Read), connection, StationDatabase.TemperatureStationtable);

                var tempStations = import.Imported;

                var rawImport = new Td3200Import(tempStations.ToArray(), percipStations.ToArray());

                var rawDatabase = new Td3200Database(connection);
                rawDatabase.CreateTables();

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                rawImport.Import(new FileStream(@"/Users/seth/Documents/LRDataSet/co05stn.dat", FileMode.Open, FileAccess.Read), connection);

                stopwatch.Stop();
            }
        }
	}
}