using System;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
	[TestFixture]
	public class StationTest
	{
		[Test]
		public void Parse() 
		{
			var line = "010008 07                  ABBA1      UNITED STATES        AL HENRY                          +6    " + 
				"ABBEVILLE 1 NNW                19910826 19980501  31 35 00 -085 17 00   465  ";
			
			var station = new Station();
			station.Parse(line);
			
			Assert.That(station.Number, Is.EqualTo("010008"));
			Assert.That(station.State, Is.EqualTo("AL"));
			Assert.That(station.StartDate, Is.EqualTo(DateTime.Parse("08/26/1991")));
			Assert.That(station.EndDate, Is.EqualTo(DateTime.Parse("05/01/1998")));
			Assert.That(station.Name, Is.EqualTo("ABBEVILLE 1 NNW"));
			
			Assert.That(station.County, Is.EqualTo("HENRY"));
			Assert.That(station.Country, Is.EqualTo("UNITED STATES"));
			Assert.That(station.Latitude, Is.EqualTo(31.583333));
			Assert.That(station.Longitude, Is.EqualTo(-85.283333));
		}
	}
}