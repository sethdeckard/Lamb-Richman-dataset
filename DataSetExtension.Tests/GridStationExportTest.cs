using System;
using System.IO;
using DataSetExtension.Database;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
    [TestFixture]
    public class GridStationExportTest
    {
        [Test]
        public void ExportTemperatureMax()
        {
            using (var connection = TestUtility.CreateConnection())
            {
                connection.Open();

                var database = new GridStationDatabase(connection);
                database.CreateSchema();
                
                var station = new GridStation() 
                {
                    GridPoint = 2,
                    Name = "TestStation1",
                    Number = "123456",
                    Latitude = 2255,
                    Longitude = 8037,
                    GridPointLatitude = 23,
                    GridPointLongitude = 80,
                    Sequence = 1,
                    RecordCount = 29292,
                    HistoricalRecordCount = 10
                };
                station.Save(connection, GridStationDatabase.TemperatureMaxStationTable);   
                
                station = new GridStation() 
                {
                    GridPoint = 2,
                    Name = "TestStation2",
                    Number = "123456",
                    Latitude = 2257,
                    Longitude = 8038,
                    GridPointLatitude = 23,
                    GridPointLongitude = 80,
                    Sequence = 0,
                    RecordCount = 978,
                    HistoricalRecordCount = 13
                };
                station.Save(connection, GridStationDatabase.TemperatureMaxStationTable);   
                
                station = new GridStation() 
                {
                    GridPoint = 2,
                    Name = "TestStation1",
                    Number = "123456",
                    Latitude = 2256,
                    Longitude = 8040,
                    GridPointLatitude = 23,
                    GridPointLongitude = 80,
                    Sequence = 3,
                    RecordCount = 44
                };
                station.Save(connection, GridStationDatabase.TemperatureMaxStationTable);
                
                var writer = new FakeGridSummaryWriter();
                var export = new GridStationExport(connection);
                export.ExportTemperatureMax(writer);
                
                Assert.That(writer.Stations.Count, Is.EqualTo(3));
                
                var actual = writer.Stations[0];
                Assert.That(actual.GridPoint, Is.EqualTo(station.GridPoint));
                Assert.That(actual.Sequence, Is.EqualTo(0));
                Assert.That(actual.HistoricalRecordCount, Is.EqualTo(13));
            }
        }
        
        [Test]
        public void ExportTemperatureMin()
        {
            using (var connection = TestUtility.CreateConnection())
            {
                connection.Open();

                var database = new GridStationDatabase(connection);
                database.CreateSchema();
                
                var station = new GridStation() 
                {
                    GridPoint = 2,
                    Name = "TestStation1",
                    Number = "123456",
                    Latitude = 2255,
                    Longitude = 8037,
                    GridPointLatitude = 23,
                    GridPointLongitude = 80,
                    Sequence = 1,
                    RecordCount = 29292
                };
                station.Save(connection, GridStationDatabase.TemperatureMinStationTable);   
                
                station = new GridStation() 
                {
                    GridPoint = 2,
                    Name = "TestStation2",
                    Number = "123456",
                    Latitude = 2257,
                    Longitude = 8038,
                    GridPointLatitude = 23,
                    GridPointLongitude = 80,
                    Sequence = 1,
                    RecordCount = 978
                };
                station.Save(connection, GridStationDatabase.TemperatureMinStationTable);   
                
                station = new GridStation() 
                {
                    GridPoint = 2,
                    Name = "TestStation1",
                    Number = "123456",
                    Latitude = 2256,
                    Longitude = 8040,
                    GridPointLatitude = 23,
                    GridPointLongitude = 80,
                    Sequence = 3,
                    RecordCount = 44
                };
                station.Save(connection, GridStationDatabase.TemperatureMinStationTable);
                
                var writer = new FakeGridSummaryWriter();
                var export = new GridStationExport(connection);
                export.ExportTemperatureMin(writer);
                
                Assert.That(writer.Stations.Count, Is.EqualTo(3));
            }           
        }
        
        [Test]
        public void ExportPrecipitation() 
        {
            using (var connection = TestUtility.CreateConnection())
            {
                connection.Open();

                var database = new GridStationDatabase(connection);
                database.CreateSchema();
                
                var station = new GridStation() 
                {
                    GridPoint = 2,
                    Name = "TestStation1",
                    Number = "123456",
                    Latitude = 2255,
                    Longitude = 8037,
                    GridPointLatitude = 23,
                    GridPointLongitude = 80,
                    Sequence = 0,
                    RecordCount = 29292
                };
                station.Save(connection, GridStationDatabase.PrecipitationStationTable);    
                
                station = new GridStation() 
                {
                    GridPoint = 2,
                    Name = "TestStation2",
                    Number = "123456",
                    Latitude = 2257,
                    Longitude = 8038,
                    GridPointLatitude = 23,
                    GridPointLongitude = 80,
                    Sequence = 1,
                    RecordCount = 978
                };
                station.Save(connection, GridStationDatabase.PrecipitationStationTable);    
                
                station = new GridStation() 
                {
                    GridPoint = 2,
                    Name = "TestStation1",
                    Number = "123456",
                    Latitude = 2256,
                    Longitude = 8040,
                    GridPointLatitude = 23,
                    GridPointLongitude = 80,
                    Sequence = 3,
                    RecordCount = 44
                };
                station.Save(connection, GridStationDatabase.PrecipitationStationTable);
                
                var writer = new FakeGridSummaryWriter();
                var export = new GridStationExport(connection);
                export.ExportPrecipitation(writer);
                
                Assert.That(writer.Stations.Count, Is.EqualTo(3));
            }               
        }
    }
}