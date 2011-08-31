using System;
using System.Data;
using System.IO;
using System.Linq;
using Mono.Data.Sqlite;
using Dapper;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
	[TestFixture]
	public class ExportControllerTest
	{
		private const string testPath = "exportcontroller-test";
		
		[SetUp]
		public void SetUp() 
		{
			Directory.CreateDirectory(testPath);
		}
		
        [Test]
        public void ExportTemperatureMin()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:;Version=3;New=True"))
            {
                connection.Open();

				var gridDb = new GridStationDatabase(connection);
				gridDb.CreateSchema();
				
				var station = new GridStation() 
				{
					Id = 4,
					GridPoint = 2
				};
			    station.Save(connection, GridStationDatabase.TemperatureMinStationTable);
				
				var td3200Db = new MeasurementDatabase(connection);
				td3200Db.CreateSchema();
				
				var stationDb = new StationDatabase(connection);
				stationDb.CreateSchema();
				
                var export = new ExportController(connection, testPath);
                export.ExportTemperatureMin(2006);

                var directories = new DirectoryInfo(testPath).GetDirectories("tmin");
				
				Assert.That(directories.Length, Is.EqualTo(1));

				Assert.That(File.Exists(Path.Combine(testPath, "tmin", "gr002")));
				
				var log = Path.Combine(testPath, "tmin-missing-2006.log");
				Assert.That(File.Exists(log));
				Assert.That(File.ReadAllText(log).Length, Is.GreaterThan(0));
            }
        }
		
		[Test]
        public void ExportTemperatureMax()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:;Version=3;New=True"))
            {
                connection.Open();

				var gridDb = new GridStationDatabase(connection);
				gridDb.CreateSchema();
				
				var station = new GridStation() 
				{
					Id = 4,
					GridPoint = 5
				};
			    station.Save(connection, GridStationDatabase.TemperatureMaxStationTable);
				
				var td3200Db = new MeasurementDatabase(connection);
				td3200Db.CreateSchema();
				
				var stationDb = new StationDatabase(connection);
				stationDb.CreateSchema();
				
                var export = new ExportController(connection, testPath);
                export.ExportTemperatureMax(2006);

                var directories = new DirectoryInfo(testPath).GetDirectories("tmax");
				
				Assert.That(directories.Length, Is.EqualTo(1));

				Assert.That(File.Exists(Path.Combine(testPath, "tmax", "gr005")));
				
				var log = Path.Combine(testPath, "tmax-missing-2006.log");
				Assert.That(File.Exists(log));
				Assert.That(File.ReadAllText(log).Length, Is.GreaterThan(0));
            }
        }
		
		[Test]
        public void ExportPrecipitation()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:;Version=3;New=True"))
            {
                connection.Open();

				var gridDb = new GridStationDatabase(connection);
				gridDb.CreateSchema();
				
				var station = new GridStation() 
				{
					Id = 4,
					GridPoint = 7
				};
			    station.Save(connection, GridStationDatabase.PrecipitationStationTable);
				
				var td3200Db = new MeasurementDatabase(connection);
				td3200Db.CreateSchema();
				
				var stationDb = new StationDatabase(connection);
				stationDb.CreateSchema();
				
                var export = new ExportController(connection, testPath);
                export.ExportPrecipitation(2006);

                var directories = new DirectoryInfo(testPath).GetDirectories("prcp");
				
				Assert.That(directories.Length, Is.EqualTo(1));

				Assert.That(File.Exists(Path.Combine(testPath, "prcp", "gr007")));
				
				var log = Path.Combine(testPath, "prcp-missing-2006.log");
				Assert.That(File.Exists(log));
				Assert.That(File.ReadAllText(log).Length, Is.GreaterThan(0));
            }
        }
		
		[TearDown]
		public void TearDown() 
		{
			if (Directory.Exists(testPath)) 
			{
				Directory.Delete(testPath, true);
			}
		}
	}
}