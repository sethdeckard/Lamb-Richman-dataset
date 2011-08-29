using System;
using Mono.Data.Sqlite;
using DataSetExtension;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
	[TestFixture]
	public class MeasurementLocatorTest
	{
		[Test]
		public void Locate()
		{
			using (var connection = TestUtility.CreateConnection())
			{
				var stationDatabase = new StationDatabase(connection);
				stationDatabase.CreateSchema();
				
				//create test stations
				var station = new Station
				{
					
				};
				station.Save(connection);
				
				var measurementDatabase = new MeasurementDatabase(connection);
				measurementDatabase.CreateSchema();
				
				//create measurements
				var measurement = new Temperature
				{
					
				};
				measurement.Save(connection, MeasurementDatabase.TemperatureMaxTable);
				
				var locator = new MeasurementLocator(connection, MeasurementDatabase.TemperatureMaxTable);
			
				var results = locator.Locate(34, 31);
				Assert.That(results.Length, Is.GreaterThan(0)); //determine number of results, inspect first record
			}
		}
	}
}