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
			var record = new Precipitation { Value = 354, DateTime = DateTime.Parse("4/6/2001") };
			Assert.That(record.ToString(2), Is.EqualTo("010406 0 C 354"));
			
			record = new Precipitation { Value = 46, DateTime = DateTime.Parse("2/23/1993") };
			Assert.That(record.ToString(0), Is.EqualTo("930223 0 A  46"));
			
			record = new Precipitation { Value = 6, DateTime = DateTime.Parse("6/6/1995") };
			Assert.That(record.ToString(1), Is.EqualTo("950606 0 B   6"));
			
			record = new Precipitation { Value = 0, DateTime = DateTime.Parse("6/6/1995") };
			Assert.That(record.ToString(1), Is.EqualTo("950606 0 B   0"));
		}
	}
}