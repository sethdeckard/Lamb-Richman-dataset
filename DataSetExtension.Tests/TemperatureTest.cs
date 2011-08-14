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
			var record = new Temperature { Value = 64, DateTime = DateTime.Parse("1/12/1949") };
			//what do we do with 00 below?
			Assert.That(record.ToString(1), Is.EqualTo("490112 00 B  178")); //17.8C
			
			record = new Temperature { Value = 81, DateTime = DateTime.Parse("2/14/1997") };
			Assert.That(record.ToString(0), Is.EqualTo("970214 00 A  272")); //27.2C
			
			record = new Temperature { Value = 23, DateTime = DateTime.Parse("3/13/2001") };
			Assert.That(record.ToString(0), Is.EqualTo("010313 00 A  -50"));  //-5C
			
			record = new Temperature { Value = 41, DateTime = DateTime.Parse("3/13/2001") };
			Assert.That(record.ToString(0), Is.EqualTo("010313 00 A   50")); //5C
			
			record = new Temperature { Value = 122, DateTime = DateTime.Parse("3/13/2001") };
			Assert.That(record.ToString(0), Is.EqualTo("010313 00 A  500")); //50C
			
			record = new Temperature { Value = 42, DateTime = DateTime.Parse("3/13/2001") };
			Assert.That(record.ToString(0), Is.EqualTo("010313 00 A   56")); //5.6C
			
			record = new Temperature { Value = 32, DateTime = DateTime.Parse("3/13/2001") };
			Assert.That(record.ToString(0), Is.EqualTo("010313 00 A    0"));
			
			record = new Temperature { Value = 21, DateTime = DateTime.Parse("2/14/1997") };
			Assert.That(record.ToString(0), Is.EqualTo("970214 00 A  -61")); //-6.1C
			
			record = new Temperature { Value = 3, DateTime = DateTime.Parse("2/14/1997") };
			Assert.That(record.ToString(0), Is.EqualTo("970214 00 A -161")); //-16.11C
			
			record = new Temperature { Value = 31, DateTime = DateTime.Parse("2/14/1997") };
			Assert.That(record.ToString(0), Is.EqualTo("970214 00 A   -6")); //-.6C
		}
	}
}