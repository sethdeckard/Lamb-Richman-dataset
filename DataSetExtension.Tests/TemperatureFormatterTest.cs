using System;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
	[TestFixture]
	public class TemperatureFormatterTest
	{
		[Test]
		public void ToStringTest()
		{
			var formater = new TemperatureFormatter();
			
			var record = new Measurement { Value = 64, Date = DateTime.Parse("1/12/1949") };
			//what do we do with 00 below?
			Assert.That(formater.Format(record, 1), Is.EqualTo("490112 00 B  178")); //17.8C
			
			record = new Measurement { Value = 81, Date = DateTime.Parse("2/14/1997") };
			Assert.That(formater.Format(record, 0), Is.EqualTo("970214 00 A  272")); //27.2C
			
			record = new Measurement { Value = 23, Date = DateTime.Parse("3/13/2001") };
			Assert.That(formater.Format(record, 0), Is.EqualTo("010313 00 A  -50"));  //-5C
			
			record = new Measurement { Value = 41, Date = DateTime.Parse("3/13/2001") };
			Assert.That(formater.Format(record, 0), Is.EqualTo("010313 00 A   50")); //5C
			
			record = new Measurement { Value = 122, Date = DateTime.Parse("3/13/2001") };
			Assert.That(formater.Format(record, 0), Is.EqualTo("010313 00 A  500")); //50C
			
			record = new Measurement { Value = 42, Date = DateTime.Parse("3/13/2001") };
			Assert.That(formater.Format(record, 0), Is.EqualTo("010313 00 A   56")); //5.6C
			
			record = new Measurement { Value = 32, Date = DateTime.Parse("3/13/2001") };
			Assert.That(formater.Format(record, 0), Is.EqualTo("010313 00 A    0"));
			
			record = new Measurement { Value = 21, Date = DateTime.Parse("2/14/1997") };
			Assert.That(formater.Format(record, 0), Is.EqualTo("970214 00 A  -61")); //-6.1C
			
			record = new Measurement { Value = 3, Date = DateTime.Parse("2/14/1997") };
			Assert.That(formater.Format(record, 0), Is.EqualTo("970214 00 A -161")); //-16.11C
			
			record = new Measurement { Value = 31, Date = DateTime.Parse("2/14/1997") };
			Assert.That(formater.Format(record, 0), Is.EqualTo("970214 00 A   -6")); //-.6C
		}
	}
}