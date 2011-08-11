using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
	[TestFixture]
	public class MeasurementExportTest
	{
        [Test]
        public void Export()
        {
            var stream = new MemoryStream();

            var stations = new List<Station>
                             {
                                 new Station { Id = 1, GridPoint = 2, Sequence = 0},
                                 new Station { Id = 2, GridPoint = 2, Sequence = 1},
                                 new Station { Id = 3, GridPoint = 2, Sequence = 2},
                                 new Station { Id = 4, GridPoint = 2, Sequence = 3},
                                 new Station { Id = 5, GridPoint = 2, Sequence = 4},
                             };

            var records = new List<IMeasurement>
                              {
                                  new Td3200 { StationId = 2, Date = DateTime.Parse("1/1/2001"), Value = 3 },
                                  new Td3200 { StationId = 4, Date = DateTime.Parse("1/2/2001"), Value = 3 },
                                  new Td3200 { StationId = 5, Date = DateTime.Parse("1/3/2001"), Value = 3 },
                                  new Td3200 { StationId = 1, Date = DateTime.Parse("1/3/2001"), Value = 3 },
                              };

            var export = new MeasurementExport(stream, stations.ToArray(), 2001);
            export.Export(records.ToArray(), 1);

            stream.Position = 0;

            using (var reader = new StreamReader(stream))
            {
                Assert.That(reader.ReadLine(), Is.EqualTo("1"));
                Assert.That(reader.ReadLine(), Is.EqualTo("3"));
                Assert.That(reader.ReadLine(), Is.EqualTo("0"));
            }

            Assert.That(export.Missing.Count, Is.EqualTo(28));
        }
	}
}