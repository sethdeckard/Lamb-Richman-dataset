﻿using DataSetExtension;
using NUnit.Framework;

namespace DataSetExtension.Test
{
    [TestFixture]
    public class StationTest 
    { 
        [Test]
        public void ParseTest()
        {
            var station = new Station();
            station.Parse("  1 25  81 Flamingo Rngr Stat, FL         083020 2509  8055   10913");

                       //Gordonsville FAA-AP,VA          443462 3804  781       7

            Assert.That(station.GridPoint, Is.EqualTo(1));
            Assert.That(station.Name, Is.EqualTo("Flamingo Rngr Stat, FL"));
            Assert.That(station.Number, Is.EqualTo("083020"));
            Assert.That(station.HistoricalRecordCount, Is.EqualTo(10913));
            Assert.That(station.RoundedLatitude, Is.EqualTo(25));
            Assert.That(station.RoundedLongitude, Is.EqualTo(81));
            Assert.That(station.Latitude, Is.EqualTo(2509));
            Assert.That(station.Longitude, Is.EqualTo(8055));
            Assert.That(station.Sequence, Is.EqualTo(0));

            station = new Station();
            station.Parse("246 38  78 Gordonsville FAA-AP,VA         443462 3804   781       7");
            Assert.That(station.GridPoint, Is.EqualTo(246));
            Assert.That(station.Name, Is.EqualTo("Gordonsville FAA-AP,VA"));
            Assert.That(station.Number, Is.EqualTo("443462"));
            Assert.That(station.HistoricalRecordCount, Is.EqualTo(7));
            Assert.That(station.RoundedLatitude, Is.EqualTo(38));
            Assert.That(station.RoundedLongitude, Is.EqualTo(78));
            Assert.That(station.Latitude, Is.EqualTo(3804));
            Assert.That(station.Longitude, Is.EqualTo(781));
            Assert.That(station.Sequence, Is.EqualTo(0));
        }
    
        [Test]
        public void ParseWithParentTest()
        {
            var parent = new Station();
            parent.Parse("  1 25  81 Flamingo Rngr Stat, FL         083020 2509  8055   10913");

            var station = new Station();
            station.Parse("           Marathin Shores, FL            085351 2444  8103    4310", parent);

            Assert.That(station.GridPoint, Is.EqualTo(1));
            Assert.That(station.Name, Is.EqualTo("Marathin Shores, FL"));
            Assert.That(station.Number, Is.EqualTo("085351"));
            Assert.That(station.HistoricalRecordCount, Is.EqualTo(4310));
            Assert.That(station.RoundedLatitude, Is.EqualTo(25));
            Assert.That(station.RoundedLongitude, Is.EqualTo(81));
            Assert.That(station.Latitude, Is.EqualTo(2444));
            Assert.That(station.Longitude, Is.EqualTo(8103));
            Assert.That(station.Sequence, Is.EqualTo(1));
        }
    }
}