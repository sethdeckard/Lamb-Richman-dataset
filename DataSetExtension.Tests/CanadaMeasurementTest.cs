using System;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
 	[TestFixture]
    public class CanadaMeasurementTest
    {
        [Test]
        public void Parse()
        {
            var line = "3010890200102002-00150 -00105 -00045 -00110 -00140 -00185 -00240 -00240 -00280 -00250 -00235" + 
                " -00200 -00190 -00175 -00175 -00295 -00250 -00230 -00190 -00250 -00185 -00225 -00160 -00200 -00295" + 
                " -00320 -00230 -00085 -99999M-99999M-99999M";
            var results = CanadaMeasurement.Parse(line);
            Assert.That(results.Length, Is.EqualTo(28));
 
            var measurement = results[0];
            Assert.That(measurement.StationNumber, Is.EqualTo("3010890"));
			Assert.That(measurement.DateTime, Is.EqualTo(DateTime.Parse("02/01/2001")));
			Assert.That(measurement.Value, Is.EqualTo(5));
			Assert.That(measurement.Element, Is.EqualTo(Element.TemperatureMin));
           
            line = "3012205200202001-00028 -00025 000015 000020 000037 000022 -00024 -00043 -00024 000050 000028 000049" + 
                " 000013 000044 000071 000026 000048 000003 -00016 000032 -00005 -00028 -00112 -00136 -00134 " + 
                "000001 -00013 -00097 -99999M-99999M-99999M";
            results = CanadaMeasurement.Parse(line);
            Assert.That(results.Length, Is.EqualTo(28));
 
            measurement = results[0];
            Assert.That(measurement.StationNumber, Is.EqualTo("3012205"));
			Assert.That(measurement.Element, Is.EqualTo(Element.TemperatureMax));
 
            line = "3012208200404012000000 000000 000000 000000 000000 000008 000000 000000 000000 000000 000000T000004" + 
                " 000028 000162 000008 000004 000000 000000 000000T000000 000000 000000T000002 000000 000000 000000" + 
                " 000100 000000T000000 000000 -99999M";
            results = CanadaMeasurement.Parse(line);
			Assert.That(results.Length, Is.EqualTo(30));
			
            measurement = results[5];
            Assert.That(measurement.StationNumber, Is.EqualTo("3012208"));
			Assert.That(measurement.DateTime, Is.EqualTo(DateTime.Parse("04/06/2004")));
			Assert.That(measurement.Value, Is.EqualTo(3));
			Assert.That(measurement.Element, Is.EqualTo(Element.Precipitation));
			
			measurement = results[10];
			Assert.That(measurement.Value, Is.EqualTo(1));
			
        }
    }
}