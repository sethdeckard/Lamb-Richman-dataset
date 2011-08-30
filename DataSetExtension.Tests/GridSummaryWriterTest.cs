using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
 	[TestFixture]
    public class GridSummaryWriterTest
    {
        [Test]
        public void WriteTest()
        {
            var stream = new MemoryStream();
            var writer = new GridSummaryWriter(stream);
 
            var items = new List<GridStation>
                            {
                                new GridStation
                                    {
                                        GridPoint = 2,
                                        Name = "TestName1",
                                        Number = "083020",
                                        RecordCount = 41042,
                                        GridPointLatitude = 35,
                                        GridPointLongitude = 88,
                                        Sequence = 0,
                                        Latitude = 2509,
                                        Longitude = 8055
                                    },
                                new GridStation
                                    {
                                        GridPoint = 2,
                                        Name = "Marathin Shores, FL",
                                        Number = "085351",
                                        RecordCount = 4310,
                                        GridPointLatitude = 35,
                                        GridPointLongitude = 88,
                                        Sequence = 0,
                                        Latitude = 2444,
                                        Longitude = 8103
                                    },
                                new GridStation
                                    {
                                        GridPoint = 2,
                                        Name = "Royal Palms Rngr Stat, FL",
                                        Number = "087760",
                                        RecordCount = 692,
                                        GridPointLatitude = 35,
                                        GridPointLongitude = 88,
                                        Sequence = 0,
                                        Latitude = 2523,
                                        Longitude = 8036
                                    },
                            };
 
            writer.Write(items.ToArray());
 
            stream.Position = 0;
            var reader = new StreamReader(stream);
 
            Assert.That(reader.ReadLine(), Is.EqualTo(""));
            Assert.That(reader.ReadLine(), Is.EqualTo("  2 35  88 TestName1                      083020 2509  8055   41042"));
            Assert.That(reader.ReadLine(), Is.EqualTo("           Marathin Shores, FL            085351 2444  8103    4310"));
            Assert.That(reader.ReadLine(), Is.EqualTo("           Royal Palms Rngr Stat, FL      087760 2523  8036     692"));
            Assert.That(reader.ReadLine(), Is.EqualTo("                                                     TOTAL:   46044"));
        }
    }
}

