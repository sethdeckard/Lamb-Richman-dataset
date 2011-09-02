using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataSetExtension
{
	public class GridPointExport
	{
        private readonly int year;
        private readonly List<GridStation> stations;
        private readonly StreamWriter writer;
		
        public GridPointExport(Stream stream, GridStation[] stations, int year)
        {
            this.stations = stations.ToList();
            this.year = year;

            writer = new StreamWriter(stream);
            Missing = new List<DateTime>();
			Added = new List<GridStation>();
        }
				
		public List<DateTime> Missing { get; set; }
		
		//get rid of added
		public List<GridStation> Added { get; set; }
		
		public IMeasurementLocator Locator { get; set; }
		
		public IFormatter Formatter { get; set; }
		
		public GridStation[] GetUpdatedStations()
		{
			throw new NotImplementedException();
			
			//returns both existing and new stations (missing lat/long/name), updated with record counts.
		}
		
		//update record counts, refactor into two methods
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
					
					if (Locator.IsNew) 
					{
						var station = new GridStation 
						{ 
							GridPoint = first.GridPoint, 
							GridPointLatitude = first.GridPointLatitude, 
							GridPointLongitude = first.GridPointLongitude, 
							Number = measurement.StationNumber,
							Sequence = sequence,
							RecordCount = 1
						};
						Added.Add(station);
						//add to stations
					}
					
					writer.WriteLine(Formatter.Format(measurement, sequence));
                }
            }

            writer.Flush();
        }	
	}
}