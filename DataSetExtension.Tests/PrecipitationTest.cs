using System;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
	[TestFixture]
	public class PrecipitationTest
	{
		[Test]
		public void ToStringTest() 
		{
			var record = new Precipitation { Value = 354, Date = DateTime.Parse("4/6/2001") };
			Assert.That(record.ToString(2), Is.EqualTo("010406 0 C 354"));
			
			record = new Precipitation { Value = 46, Date = DateTime.Parse("2/23/1993") };
			Assert.That(record.ToString(0), Is.EqualTo("930223 0 A  46"));
		}
	}
}