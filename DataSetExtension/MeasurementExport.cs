using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataSetExtension
{
	public class MeasurementMissingEventArgs : EventArgs
	{
		public DateTime Date { get; set; }	
	}
		
	public class MeasurementExport
	{
        private readonly int year;
        private readonly GridStation[] stations;
        private readonly StreamWriter writer;
		
		public event EventHandler<MeasurementMissingEventArgs> MeasurementMissing;
		
        public MeasurementExport(Stream stream, GridStation[] stations, int year)
        {
            this.stations = stations;
            this.year = year;

            writer = new StreamWriter(stream);
            Missing = new List<DateTime>();
        }
				
		public List<DateTime> Missing { get; set; }
		
		public IMeasurementLocator Locator { get; set; }

        public void Write(IMeasurement[] records, int month)
        {
            for (var day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                var date = new DateTime(year, month, day);
                var found = false;
				long sequence = 0;
                foreach (var station in stations.OrderBy(station => station.Sequence))
                {
					sequence = station.Sequence;
					
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
					if (stations.Count() == 0)
					{
						continue;
					}
					
					//todo: get rid of event approach
					var first = stations.First();
					
					var results = Locator.Find(first.GridPointLatitude, first.GridPointLongitude, date);
					if (results.Length == 0)
					{
						OnMeasurementMissing(new MeasurementMissingEventArgs { Date = date });
						Missing.Add(date);	
						
						continue;
					}
					
					//writer.WriteLine(results.First().ToString(sequence));
                }
            }

            writer.Flush();
        }
		
        protected virtual void OnMeasurementMissing(MeasurementMissingEventArgs e)
        {
            EventHandler<MeasurementMissingEventArgs> handler = MeasurementMissing;

            if (handler != null)
            {
                handler(this, e);
            }
        }		
	}
}