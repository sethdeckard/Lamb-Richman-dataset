using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
	[TestFixture]
	public class MeasurementWriterTest
	{
        [Test]
        public void Write()
        {
            var stream = new MemoryStream();

            var stations = new List<GridStation>
                             {
                                 new GridStation { Id = 1, GridPoint = 2, Sequence = 0},
                                 new GridStation { Id = 2, GridPoint = 2, Sequence = 1},
                                 new GridStation { Id = 3, GridPoint = 2, Sequence = 2},
                                 new GridStation { Id = 4, GridPoint = 2, Sequence = 3},
                                 new GridStation { Id = 5, GridPoint = 2, Sequence = 4},
                             };

            var records = new List<IMeasurement>
                              {
                                  new Td3200 { StationId = 2, Date = DateTime.Parse("1/1/2001"), Value = 3 },
                                  new Td3200 { StationId = 4, Date = DateTime.Parse("1/2/2001"), Value = 3 },
                                  new Td3200 { StationId = 5, Date = DateTime.Parse("1/3/2001"), Value = 3 },
                                  new Td3200 { StationId = 1, Date = DateTime.Parse("1/3/2001"), Value = 3 },
                              };
			
			var locator = new FakeMeasurementLocator();
			var formatter = new SequenceFormatter();
			
            var writer = new MeasurementWriter(stream, stations.ToArray(), 2001) { Locator = locator, Formatter = formatter };
            writer.Write(records.ToArray(), 1);

            stream.Position = 0;

            using (var reader = new StreamReader(stream))
            {
                Assert.That(reader.ReadLine(), Is.EqualTo("1"));
                Assert.That(reader.ReadLine(), Is.EqualTo("3"));
                Assert.That(reader.ReadLine(), Is.EqualTo("0"));
            }
			
			Assert.That(writer.Missing.Count, Is.EqualTo(0));
        }

        [Test]
        public void WriteMissing()
        {
            var stream = new MemoryStream();

            var stations = new List<GridStation>
                             {
                                 new GridStation { Id = 1, GridPoint = 2, Sequence = 0},
                                 new GridStation { Id = 2, GridPoint = 2, Sequence = 1},
                                 new GridStation { Id = 3, GridPoint = 2, Sequence = 2},
                                 new GridStation { Id = 4, GridPoint = 2, Sequence = 3},
                                 new GridStation { Id = 5, GridPoint = 2, Sequence = 4},
                             };

            var records = new List<IMeasurement>
                              {
                                  new Td3200 { StationId = 2, Date = DateTime.Parse("1/1/2001"), Value = 3 },
                                  new Td3200 { StationId = 4, Date = DateTime.Parse("1/2/2001"), Value = 3 },
                                  new Td3200 { StationId = 5, Date = DateTime.Parse("1/3/2001"), Value = 3 },
                                  new Td3200 { StationId = 1, Date = DateTime.Parse("1/3/2001"), Value = 3 },
                              };
			
			var locator = new FakeMeasurementLocator() { PassNull = true };
			var formatter = new SequenceFormatter();
			
            var writer = new MeasurementWriter(stream, stations.ToArray(), 2001) { Locator = locator, Formatter = formatter };
            writer.Write(records.ToArray(), 1);

            stream.Position = 0;

            using (var reader = new StreamReader(stream))
            {
                Assert.That(reader.ReadLine(), Is.EqualTo("1"));
                Assert.That(reader.ReadLine(), Is.EqualTo("3"));
                Assert.That(reader.ReadLine(), Is.EqualTo("0"));
            }

			Assert.That(writer.Missing.Count, Is.EqualTo(28));
        }
		
		[Test]
		public void GetUpdatedStations() 
		{
            var stream = new MemoryStream();

            var stations = new List<GridStation>
                             {
                                 new GridStation { Id = 1, GridPoint = 2, Sequence = 0, RecordCount = 100 },
                                 new GridStation { Id = 2, GridPoint = 2, Sequence = 1, RecordCount = 100 },
                                 new GridStation { Id = 3, GridPoint = 2, Sequence = 2},
                                 new GridStation { Id = 4, GridPoint = 2, Sequence = 3, RecordCount = 100 },
                                 new GridStation { Id = 5, GridPoint = 2, Sequence = 4},
                             };

            var records = new List<IMeasurement>
                              {
                                  new Td3200 { StationId = 2, Date = DateTime.Parse("1/1/2001"), Value = 3 },
                                  new Td3200 { StationId = 4, Date = DateTime.Parse("1/2/2001"), Value = 3 },
                                  new Td3200 { StationId = 5, Date = DateTime.Parse("1/3/2001"), Value = 3 },
                                  new Td3200 { StationId = 1, Date = DateTime.Parse("1/3/2001"), Value = 3 },
                              };
			
			var locator = new FakeMeasurementLocator();
			var formatter = new SequenceFormatter();
			
            var writer = new MeasurementWriter(stream, stations.ToArray(), 2001) { Locator = locator, Formatter = formatter };
            writer.Write(records.ToArray(), 1);
			
			var updated = writer.GetUpdatedStations();
			
			Assert.That(updated.Length, Is.EqualTo(33));
			
			var count = (from station in updated
						where station.IsNew
						select station).Count();
			
			Assert.That(count, Is.EqualTo(28));
			
			count = (from station in updated
					where station.RecordCount == 101
					select station).Count();
			
			Assert.That(count, Is.EqualTo(3));
			
			count = (from station in updated
				  	where station.RecordCount == 0
				  	select station).Count();
			
			Assert.That(count, Is.EqualTo(2));
		}
	}
}