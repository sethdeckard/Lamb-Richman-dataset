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
		
		public List<DateTime> Missing { get; set; }

        public MeasurementExport(Stream stream, GridStation[] stations, int year)
        {
            this.stations = stations;
            this.year = year;

            writer = new StreamWriter(stream);
            Missing = new List<DateTime>();
        }

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
					//TODO: QueryMeasurementLocator, if no records are returned then add to missign collection,
					//get rid of event approach
					
					
					OnMeasurementMissing(new MeasurementMissingEventArgs { Date = date });
					Missing.Add(date);
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