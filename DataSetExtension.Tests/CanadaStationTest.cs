using System;
using DataSetExtension;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
	[TestFixture]
	public class CanadaStationTest
	{
		[Test]
		public void Parse()
		{
			var station = new CanadaStation();
			station.Parse("3010410	Aurora Lo	52.65	115.7166667");
			
			Assert.That(station.Number, Is.EqualTo("3010410"));
			Assert.That(station.Name, Is.EqualTo("Aurora Lo"));
			Assert.That(station.Latitude, Is.EqualTo(52.65M));
			Assert.That(station.Longitude, Is.EqualTo(115.7166667M));
			Assert.That(station.Start.Date, Is.EqualTo(new DateTime(1, 1, 1)));
		}
	}
}