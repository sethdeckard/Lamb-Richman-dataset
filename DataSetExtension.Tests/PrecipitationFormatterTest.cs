using System;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
    [TestFixture]
    public class PrecipitationFormatterTest
    {
        [Test]
        public void ToStringTest()
        {
            var formater = new PrecipitationFormatter();
            
            var record = new Measurement { Value = 354, Date = DateTime.Parse("4/6/2001"), ObservationHour = 18 };
            Assert.That(formater.Format(record, 2), Is.EqualTo("010406 18 C 354"));
            
            record = new Measurement { Value = 46, Date = DateTime.Parse("2/23/1993"), ObservationHour = 6 };
            Assert.That(formater.Format(record, 0), Is.EqualTo("930223  6 A  46"));
            
            record = new Measurement { Value = 6, Date = DateTime.Parse("6/6/1995"), ObservationHour = 5 };
            Assert.That(formater.Format(record, 1), Is.EqualTo("950606  5 B   6"));
            
            record = new Measurement { Value = 0, Date = DateTime.Parse("6/6/1995"), ObservationHour = 14 };
            Assert.That(formater.Format(record, 1), Is.EqualTo("950606 14 B   0"));
            
            record = new Measurement { Date = DateTime.Parse("1/2/2011"), ObservationHour = 5 };
            Assert.That(formater.Format(record, -1), Is.EqualTo("110102  5 0   0"));
        }
    }
}