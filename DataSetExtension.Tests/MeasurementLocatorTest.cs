using System;
using System.Linq;
using Mono.Data.Sqlite;
using DataSetExtension;
using DataSetExtension.Database;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
    [TestFixture]
    public class MeasurementLocatorTest
    {
        [Test]
        public void Find()
        {
            using (var connection = TestUtility.CreateConnection())
            {
                connection.Open();
                
                var stationDatabase = new StationDatabase(connection);
                stationDatabase.CreateSchema();
                
                var station = new Station
                {
                    Number = "445591",
                    Latitude = 32.4473874656245M,
                    Longitude = -83.3134799242268M,
                    Start = DateTime.Parse("7/30/2012")
                };
                station.Save(connection);
                
                station = new Station
                {
                    Number = "123",
                    Latitude = 29.5527M,
                    Longitude = -87.6718866M,
                    Start = DateTime.Parse("7/30/2010")
                };
                station.Save(connection);
                
                station = new Station
                {
                    Number = "445595",
                    Latitude = 31.05M,
                    Longitude = -85.30M,
                    Start = DateTime.Parse("11/30/2009"),
                    End = DateTime.Parse("1/30/2010")
                };
                station.Save(connection);
                
                station = new Station
                {
                    Number = "445599",
                    Latitude = 31.01M,
                    Longitude = -85.1M,
                    Start = DateTime.Parse("11/30/2010")
                };
                station.Save(connection);
                
                station = new Station
                {
                    Number = "445500",
                    Latitude = 31.06M,
                    Longitude = -85.30M,
                    Start = DateTime.Parse("11/30/2010")
                };
                station.Save(connection);
                
                station = new Station
                {
                    Number = "xxx",
                    Latitude = 32.44737M,
                    Longitude = -82.328119M,
                    Start = DateTime.Parse("11/30/2010")
                };
                station.Save(connection);
                                    
                var measurementDatabase = new MeasurementDatabase(connection);
                measurementDatabase.CreateSchema();
                
                var measurement = new Measurement
                {
                    Date = DateTime.Parse("1/31/2010"),
                    StationNumber = "445599"
                };
                measurement.Save(connection, MeasurementDatabase.TemperatureMaxTable);
                
                measurement = new Measurement
                {
                    Date = DateTime.Parse("12/31/2010"),
                    StationNumber = "445500",
                    Value = 1
                };
                measurement.Save(connection, MeasurementDatabase.TemperatureMaxTable);
                
                measurement = new Measurement
                {
                    Date = DateTime.Parse("12/31/2010"),
                    StationNumber = "123",
                    Value = 1
                };
                measurement.Save(connection, MeasurementDatabase.TemperatureMaxTable);
                
                measurement = new Measurement
                {
                    Date = DateTime.Parse("12/31/2010"),
                    StationNumber = "xxx",
                    Value = 1
                };
                measurement.Save(connection, MeasurementDatabase.TemperatureMaxTable);
                
                measurement = new Measurement
                {
                    Date = DateTime.Parse("12/31/2010"),
                    StationNumber = "445599",
                    Value = 2,
                    ObservationHour = 13
                };
                measurement.Save(connection, MeasurementDatabase.TemperatureMaxTable);
                
                measurement = new Measurement
                {
                    Date = DateTime.Parse("12/30/2010"),
                    StationNumber = "445599"
                };
                measurement.Save(connection, MeasurementDatabase.TemperatureMaxTable);
                
                var tracker = new StationTracker();
                var locator = new MeasurementLocator(connection, MeasurementDatabase.TemperatureMaxTable, tracker);         
            
                var date = DateTime.Parse("12/31/2010");
                var result = locator.Find(31, 85, date);
                
                Assert.That(locator.IsNew, Is.True);
                
                Assert.That(result.StationNumber, Is.EqualTo("445599"));
                Assert.That(result.Date, Is.EqualTo(date));
                Assert.That(result.ObservationHour, Is.EqualTo(13));
                Assert.That(result.Value, Is.EqualTo(2));
                
                result = locator.Find(31, 85, date);
                Assert.That(result.StationNumber, Is.EqualTo("445500"));
                
                result = locator.Find(31, 85, date);
                Assert.That(result.StationNumber, Is.EqualTo("xxx"));
                
                result = locator.Find(31, 85, date);
                Assert.That(result.StationNumber, Is.EqualTo("123"));
            
                Assert.That(locator.Find(31, 85, date), Is.Null);
                
                //todo, consider using mock tracker
            }
        }
    }
}