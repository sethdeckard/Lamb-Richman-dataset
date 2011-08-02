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
			//630403 6 C 0
			var record = new Precipitation { Value = 354, Date = DateTime.Parse("4/6/2001") };
			
			Assert.That(record.ToString(2), Is.EqualTo("010406 0 C 354"));
		}
	}
}