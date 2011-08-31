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
			var formater = new PrecipitationFormatter();
			
			var record = new Measurement { Value = 354, Date = DateTime.Parse("4/6/2001") };
			Assert.That(formater.Format(record, 2), Is.EqualTo("010406 0 C 354"));
			
			record = new Measurement { Value = 46, Date = DateTime.Parse("2/23/1993") };
			Assert.That(formater.Format(record, 0), Is.EqualTo("930223 0 A  46"));
			
			record = new Measurement { Value = 6, Date = DateTime.Parse("6/6/1995") };
			Assert.That(formater.Format(record, 1), Is.EqualTo("950606 0 B   6"));
			
			record = new Measurement { Value = 0, Date = DateTime.Parse("6/6/1995") };
			Assert.That(formater.Format(record, 1), Is.EqualTo("950606 0 B   0"));
		}
	}
}