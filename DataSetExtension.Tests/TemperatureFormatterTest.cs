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
            Assert.That(formater.Format(record, 1), Is.EqualTo("49 112  0 B  178")); //17.8C
            
            record = new Measurement { Value = 81, Date = DateTime.Parse("2/14/1997"), ObservationHour = 12 };
            Assert.That(formater.Format(record, 0), Is.EqualTo("97 214 12 A  272")); //27.2C
            
            record = new Measurement { Value = 23, Date = DateTime.Parse("3/4/2001"), ObservationHour = 6 };
            Assert.That(formater.Format(record, 0), Is.EqualTo(" 1 3 4  6 A  -50"));  //-5C
            
            record = new Measurement { Value = 41, Date = DateTime.Parse("3/13/2001"), ObservationHour = 8 };
            Assert.That(formater.Format(record, 0), Is.EqualTo(" 1 313  8 A   50")); //5C
            
            record = new Measurement { Value = 122, Date = DateTime.Parse("3/13/2001"), ObservationHour = 16 };
            Assert.That(formater.Format(record, 0), Is.EqualTo(" 1 313 16 A  500")); //50C
            
            record = new Measurement { Value = 42, Date = DateTime.Parse("3/13/2001"), ObservationHour = 4 };
            Assert.That(formater.Format(record, 0), Is.EqualTo(" 1 313  4 A   56")); //5.6C
            
            record = new Measurement { Value = 32, Date = DateTime.Parse("3/13/2001"), ObservationHour = 20 };
            Assert.That(formater.Format(record, 0), Is.EqualTo(" 1 313 20 A    0"));
            
            record = new Measurement { Value = 21, Date = DateTime.Parse("2/14/1997"), ObservationHour = 1 };
            Assert.That(formater.Format(record, 0), Is.EqualTo("97 214  1 A  -61")); //-6.1C
            
            record = new Measurement { Value = 3, Date = DateTime.Parse("2/14/1997"), ObservationHour = 6 };
            Assert.That(formater.Format(record, 0), Is.EqualTo("97 214  6 A -161")); //-16.11C
            
            record = new Measurement { Value = 31, Date = DateTime.Parse("2/14/1997"), ObservationHour = 5 };
            Assert.That(formater.Format(record, 0), Is.EqualTo("97 214  5 A   -6")); //-.6C
            
            record = new Measurement { Value = 31, Date = DateTime.Parse("2/14/1997"), ObservationHour = 5 };
            Assert.That(formater.Format(record, -1), Is.EqualTo("97 214  5 0   -6")); //-.6C
        }
    }
}