using DataSetExtension;
using NUnit.Framework;

namespace DataSetExtension.Test
{
    [TestFixture]
    public class TemperatureStationTest
    {
        [Test]
        public void ParseTest()
        {
            var station = new TemperatureStation();
            station.Parse("  1 25  81 Flamingo Rngr Stat, FL         083020 2509  8055   10913");

            Assert.That(station.GridPoint, Is.EqualTo(1));
            Assert.That(station.Name, Is.EqualTo("Flamingo Rngr Stat, FL"));
            Assert.That(station.Number, Is.EqualTo("083020"));
            Assert.That(station.HistoricalRecordCount, Is.EqualTo(10913));
            Assert.That(station.RoundedLatitude, Is.EqualTo(25));
            Assert.That(station.RoundedLongitude, Is.EqualTo(81));
            Assert.That(station.Latitude, Is.EqualTo(2509));
            Assert.That(station.Longitude, Is.EqualTo(8055));
            Assert.That(station.Sequence, Is.EqualTo(0));
        }
    }
}