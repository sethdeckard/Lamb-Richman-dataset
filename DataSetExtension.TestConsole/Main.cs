using System;
using System.Data;
using System.Data.SQLite;
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
            using (IDbConnection connection = new SQLiteConnection(@"Data Source=C:\Users\sdeckard\Documents\stations.sqlite;Version=3"))
            {
                connection.Open();

                var database = new StationDatabase(connection);
                database.CreateTables();

                var import = new StationImport();
                import.Import(new FileStream(@"C:\Users\sdeckard\Documents\prcpinfo.txt", FileMode.Open, FileAccess.Read), connection, StationDatabase.PrecipitationStationTable);

                var percipStations = import.Imported;

                import = new StationImport();
                import.Import(new FileStream(@"C:\Users\sdeckard\Documents\tmaxinfo.txt", FileMode.Open, FileAccess.Read), connection, StationDatabase.TemperatureStationtable);

                var tempStations = import.Imported;

                var rawImport = new Td3200Import(tempStations.ToArray(), percipStations.ToArray());

                var rawDatabase = new Td3200Database(connection);
                rawDatabase.CreateTables();

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                rawImport.Import(new FileStream(@"C:\Users\sdeckard\Documents\co05stn.dat", FileMode.Open, FileAccess.Read), connection);

                stopwatch.Stop();
            }
        }
	}
}