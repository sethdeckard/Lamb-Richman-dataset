using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataSetExtension
{
	public class MeasurementExport
	{
        private readonly int year;
        private readonly GridStation[] stations;
        private readonly StreamWriter writer;
		
        public MeasurementExport(Stream stream, GridStation[] stations, int year)
        {
            this.stations = stations;
            this.year = year;

            writer = new StreamWriter(stream);
            Missing = new List<DateTime>();
        }
				
		public List<DateTime> Missing { get; set; }
		
		public IMeasurementLocator Locator { get; set; }
		
		public IFormatter Formatter { get; set; }

        public void Write(IMeasurement[] records, int month)
        {
            for (var day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                var date = new DateTime(year, month, day);
                var found = false;
				long sequence = 0;
                foreach (var station in stations.OrderBy(station => station.Sequence))
                {
                    var query = from record in records 
                                where record.Date == date && record.StationId == station.Id 
                                select record;

                    if (query.Count() > 0)
                    {
                       	writer.WriteLine(Formatter.Format(query.First(), station.Sequence));
                        found = true;
                        break;
                    }
				
					sequence = station.Sequence;
                }

                if (!found && stations.Count() > 0)
                {					
					var first = stations.First();
					
					var measurement = Locator.Find(first.GridPointLatitude, first.GridPointLongitude, date);
					if (measurement == null)
					{
						Missing.Add(date);	
						
						continue;
					}
					
					writer.WriteLine(Formatter.Format(measurement, sequence));
                }
            }

            writer.Flush();
        }		
	}
}