using System;
using DataSetExtension;
using NUnit.Framework;

namespace DataSetExtension.Tests
{ 
    [TestFixture]
    public class Td3200Test
    {
        [Test]
        public void ParseTemperatureMax()
        {
            var record = "0370DLY05504802TMAX F19610299990310118 00048 10218-00050 10318 00045 10418 00035 10518 00034 " +
                         "10618 00041 10718 00040 10818 00044 10918 00049 11018 00058 11118 00057 11218 00054 11318 00048 11418 " +
                         "00051 11518 00058 11618 00055 11718 00047 11818 00039 11918 00043J12018 00042T12118 00052 12218 00051" +
                         " 12318 00032 12418 00046 12518 00050 02618 00040 K2718 00036 12818 00041 12999-99999M 3099-99999M 3199-99999M";

            var results = Td3200.Parse(record);

            Assert.That(results.Length, Is.EqualTo(27));

            var first = results[0];

            Assert.That(first.StationNumber, Is.EqualTo("055048"));

            Assert.That(first.Date, Is.EqualTo(DateTime.Parse("02/01/1961")));
            
            Assert.That(first.ObservationHour, Is.EqualTo(18));

            Assert.That(first.Value, Is.EqualTo(48));

            var second = results[1];

            Assert.That(second.Value, Is.EqualTo(-50));
        }

        [Test]
        public void ParseTemperatureMin()
        {
            var record = "0370DLY05504802TMIN F19610299990310118 00048 10218-00050 10318 00045 10418 00035 10518 00034 " +
                         "10618 00041 10718 00040 10818 00044 10918 00049 11018 00058 11118 00057 11218 00054 11318 00048 11418 " +
                         "00051 11518 00058 11618 00055 11718 00047 11818 00039 11918 00043J12018 00042T12118 00052 12218 00051" +
                         " 12318 00032 12418 00046 12518 00050 02618 00040 K2718 00036 12818 00041 12999-99999M 3099-99999M 3199-99999M";

            var results = Td3200.Parse(record);

            Assert.That(results.Length, Is.EqualTo(27));

            var first = results[0];

            Assert.That(first.StationNumber, Is.EqualTo("055048"));

            Assert.That(first.Date, Is.EqualTo(DateTime.Parse("02/01/1961")));
            
            Assert.That(first.ObservationHour, Is.EqualTo(18));

            Assert.That(first.Value, Is.EqualTo(48));

            var second = results[1];

            Assert.That(second.Value, Is.EqualTo(-50));
        }

        [Test]
        public void ParsePrecipitation()
        {
            var record = "0370DLY05160902PRCPHI19710999990310118 00000 10218 00010 10318 00040 10418 00000 10518 00000 " +
                         "10618 00000 10718 00000 10818 00000 10918 00000 11018 00000 11118 00000 11218 00000 11318 00000 11418 " + 
                         "00000 11518 00000 11618 00000 11718 00055 11818 00000 11918 00000 12018 00000 12118 00000 12218 00000 " + 
                         "12318 00000 12418 00000 12518 00000 12618 00000 12718 00000 12818 00000 12918 00000 13018 00000 13199-99999M";

            var results = Td3200.Parse(record);

            Assert.That(results.Length, Is.EqualTo(30));

            var first = results[0];

            Assert.That(first.StationNumber, Is.EqualTo("051609"));

            Assert.That(first.Date, Is.EqualTo(DateTime.Parse("09/01/1971")));
            
            Assert.That(first.ObservationHour, Is.EqualTo(18));

            Assert.That(first.Value, Is.EqualTo(0));

            var second = results[1];

            Assert.That(second.Value, Is.EqualTo(10));
        }
    }
}