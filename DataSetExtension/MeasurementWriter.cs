using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataSetExtension
{
	public class MeasurementWriter
	{
        private readonly int year;
        private readonly List<GridStation> stations;
        private readonly StreamWriter writer;
		
        public MeasurementWriter(Stream stream, GridStation[] stations, int year)
        {
            this.stations = stations.ToList();
            this.year = year;

            writer = new StreamWriter(stream);
            Missing = new List<DateTime>();
        }
				
		public List<DateTime> Missing { get; set; }
		
		public IMeasurementLocator Locator { get; set; }
		
		public IFormatter Formatter { get; set; }
		
		public GridStation[] GetUpdatedStations()
		{
			return stations.ToArray();
		}
		
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
                                where record.Date == date && record.StationId == station.Id &&
								Locator.Tracker.Validate(record.StationNumber, date)
                                select record;

                    if (query.Count() > 0)
                    {
						var record = query.First();
						
						Locator.Tracker.Update(record.StationNumber, date);
                       	
						writer.WriteLine(Formatter.Format(record, station.Sequence));
						
						station.RecordCount += 1;
						
                        found = true;
                        break;
                    }
				
					sequence = station.Sequence;
                }

                if (!found && stations.Count() > 0)
                {					
					WriteMissing(stations.First(), date, sequence + 1);	
				}
            }

            writer.Flush();
        }	
		
		private void WriteMissing(GridStation first, DateTime date, long sequence)
		{
			var measurement = Locator.Find(first.GridPointLatitude, first.GridPointLongitude, date);
			if (measurement == null)
			{
				Missing.Add(date);	
				
				return;
			}
			
			int index = stations.FindIndex(station => station.Number == measurement.StationNumber);
			if (index < 0) 
			{
				var station = new GridStation 
				{ 
					GridPoint = first.GridPoint, 
					GridPointLatitude = first.GridPointLatitude, 
					GridPointLongitude = first.GridPointLongitude, 
					Number = measurement.StationNumber,
					Sequence = sequence,
					RecordCount = 1,
					IsNew = true
				};
				
				stations.Add(station);
			} 
			else 
			{
				stations[index].RecordCount += 1;
			}
			
			writer.WriteLine(Formatter.Format(measurement, sequence));			
		}
	}
}