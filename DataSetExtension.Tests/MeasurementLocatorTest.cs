using System;
using System.Linq;
using Mono.Data.Sqlite;
using DataSetExtension;
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
					Latitude = 34.05M,
					Longitude = 31.30M,
					Start = DateTime.Parse("7/30/2012")
				};
				station.Save(connection);
				
				station = new Station
				{
					Number = "445595",
					Latitude = 34.05M,
					Longitude = 31.30M,
					Start = DateTime.Parse("11/30/2009"),
					End = DateTime.Parse("1/30/2010")
				};
				station.Save(connection);
				
				station = new Station
				{
					Number = "445599",
					Latitude = 34.05M,
					Longitude = 31.30M,
					Start = DateTime.Parse("11/30/2010")
				};
				station.Save(connection);
				
				var measurementDatabase = new MeasurementDatabase(connection);
				measurementDatabase.CreateSchema();
				
				var measurement = new Measurement
				{
					Date = DateTime.Parse("12/31/2010"),
					StationNumber = "445599"
				};
				measurement.Save(connection, MeasurementDatabase.TemperatureMaxTable);
				
				var tracker = new StationTracker();
				var locator = new MeasurementLocator(connection, MeasurementDatabase.TemperatureMaxTable) { Tracker = tracker };			
			
				var date = DateTime.Parse("12/31/2010");
				var result = locator.Find(34, 31, date);
				
				Assert.That(locator.IsNew, Is.True);
				
				Assert.That(result.StationNumber, Is.EqualTo("445599"));
				Assert.That(result.Date, Is.EqualTo(date));
			}
		}
	}
}