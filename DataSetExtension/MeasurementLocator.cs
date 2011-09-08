using System;
using System.Data;
using System.IO;
using System.Linq;
using Dapper;

namespace DataSetExtension
{
	public class MeasurementLocator : IMeasurementLocator
	{
		private const int MileRadius = 100;
		private readonly IDbConnection connection;
		private readonly string query;
	
		public MeasurementLocator()
		{
		}
		
		public MeasurementLocator(IDbConnection connection, string table, StationTracker tracker)
		{
			this.connection = connection;
			Tracker = tracker;	
			query = CreateQuery(table);
		}
		
		public StationTracker Tracker { get; set; }
		
		public bool IsNew { get; set; }
		
		public virtual Measurement Find(double latitude, double longitude, DateTime date)
		{
			var boundry = GetBoundry(latitude, longitude * -1);
			
			var parameters = new 
			{ 
				MinLatitude = boundry.MinLatitude, 
				MaxLatitude = boundry.MaxLatitude, 
				MinLongitude = boundry.MinLongitude,
				MaxLongitude = boundry.MaxLongitude, 
				Date = date
			};
	
			var results = connection.Query<Measurement, Station, Measurement>(
			query, 
			(measurement, station) => { measurement.Station = station; return measurement; }, 
			parameters,
			splitOn: "Number, StationNumber");
			
			Measurement[] matches = (from measurement in results
									orderby measurement.Station.CalculateDistance(Convert.ToDouble(latitude), Convert.ToDouble(longitude) * -1)
									select measurement).ToArray();
			
			foreach (var measurement in matches.Where(measurement => Tracker.Validate(measurement.StationNumber, measurement.Date)))
            {
                IsNew = Tracker.Update(measurement.StationNumber, date);
				
                return measurement;
            }
 
            return null;
		}
		
        private static Boundry GetBoundry(double latitude, double longitude)
        {
            var latitudeRadius = MileRadius / 69.09;
			var longitudeRadius = MileRadius / (Math.Cos(latitude) * 69.172);
 
            return new Boundry 
            { 
                MinLatitude = latitude - latitudeRadius, 
                MaxLatitude = latitude + latitudeRadius, 
                MinLongitude = longitude - longitudeRadius, 
                MaxLongitude = longitude + longitudeRadius 
            };
        }	

		static string CreateQuery(string table)
		{
			var writer = new StringWriter();
			writer.WriteLine("select m.Id, m.StationId, m.StationNumber, m.Date, m.Value, m.Id,");
			writer.WriteLine("s.Id, s.Number, s.Name, s.State, s.County, s.Latitude, s.Longitude, s.Start, s.End");
			writer.WriteLine("from Station s inner join {0} m on s.Number = m.StationNumber", table);
			writer.WriteLine("where Latitude >= @MinLatitude and Latitude <= @MaxLatitude");
			writer.WriteLine("and Longitude >= @MinLongitude and Longitude <= @MaxLongitude");
			writer.WriteLine("and Date = @Date");
			writer.WriteLine("and Start <= @Date");
			writer.WriteLine("and (End is null or End >= @Date);");
			return writer.ToString();
		}
	}
}