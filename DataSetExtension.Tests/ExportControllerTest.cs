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
		[Test]
		public void SetUp() 
		{
			new DirectoryInfo(Path.GetTempPath()).CreateSubdirectory("datasetextension-export-test");
		}
		
        [Test]
        public void ExportTemperatureMin()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:;Version=3;New=True"))
            {
                connection.Open();

				var stationDb = new StationDatabase(connection);
				stationDb.CreateSchema();
				
				var station = new Station() 
				{
					Id = 4,
					GridPoint = 2
				};
			    station.Save(connection, StationDatabase.TemperatureStationtable);
				
				var td3200Db = new Td3200Database(connection);
				td3200Db.CreateSchema();
				
               	
				
                var path = new DirectoryInfo(Path.GetTempPath()).CreateSubdirectory(Guid.NewGuid() + "export-test").FullName;

                var export = new ExportController(connection, path);

                export.ExportTemperatureMin(2006);

                //inspect files
            }
        }
		
		[Test]
		public void TearDown() 
		{
			new DirectoryInfo(Path.GetTempPath()).CreateSubdirectory("datasetextension-export-test");
		}
	}
}

