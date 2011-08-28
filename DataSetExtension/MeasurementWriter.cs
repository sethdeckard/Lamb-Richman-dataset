using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataSetExtension
{
	public class MeasurementWriter
	{
        private readonly int year;
        private readonly GridStation[] stations;
        private readonly StreamWriter writer;

        public MeasurementWriter(Stream stream, GridStation[] stations, int year)
        {
            this.stations = stations;
            this.year = year;

            writer = new StreamWriter(stream);
            Missing = new List<DateTime>();
        }

        public List<DateTime> Missing { get; set; }

        public void Write(IMeasurement[] records, int month)
        {
            for (var day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                var date = new DateTime(year, month, day);
                var found = false;
                foreach (var station in stations.OrderBy(station => station.Sequence))
                {
                    var query = from record in records 
                                where record.Date == date && record.StationId == station.Id 
                                select record;

                    if (query.Count() > 0)
                    {
                        writer.WriteLine(query.First().ToString(station.Sequence));
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Missing.Add(date);
                }
            }

            writer.Flush();
        }
	}
}