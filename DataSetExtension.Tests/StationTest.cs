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
			Assert.That(station.Start, Is.EqualTo(DateTime.Parse("08/26/1991")));
			Assert.That(station.End, Is.EqualTo(DateTime.Parse("05/01/1998")));
			Assert.That(station.Name, Is.EqualTo("ABBEVILLE 1 NNW"));
			
			Assert.That(station.County, Is.EqualTo("HENRY"));
			Assert.That(station.Country, Is.EqualTo("UNITED STATES"));
			Assert.That(station.Longitude, Is.EqualTo(-85.283333M));
			Assert.That(station.Latitude, Is.EqualTo(31.583333M));
			
			station = new Station();
			station.Parse("092570 07                  DAWG1      UNITED STATES        GA TERRELL                        +5    " + 
				"DAWSON                         20021002 99991231  31 46 55 -084 26 59   355  ");
			
			Assert.That(station.End, Is.Null);
		}
	}
}