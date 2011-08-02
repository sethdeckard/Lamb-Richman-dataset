using System;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
	[TestFixture]
	public class TemperatureTest
	{
		[Test]
		public void ToStringTest()
		{
			var record = new Temperature { Value = 64, Date = DateTime.Parse("1/12/1949") };
			
			//what do we do with 00 below?
			Assert.That(record.ToString(1), Is.EqualTo("49 1 12 00 B 177"));
		}
	}
}

