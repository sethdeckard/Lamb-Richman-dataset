using System;
using System.Data;
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
			
			this.query = "select StationNumber, Date, Value from " + table + 
				" inner join Station s on s.Number = StationNumber " +
				" where Latitude > @MinLatitude and Latitude < @MaxLatitude" +
				" and Longitude > @MinLongitude and Longitude < @MaxLongitude;";
			
			//todo locator should either load up all GridStations first or do a query to make sure it isn't used already
		}
		
		public StationTracker Tracker { get; set; }
		
		public bool IsNew { get; set; }
		
		public virtual Measurement Find(decimal latitude, decimal longitude, DateTime date)
		{
			var boundry = GetBoundry(latitude, longitude);
			
			var parameters = new 
			{ 
				MinLatitude = boundry.MinLatitude, 
				MaxLatitude = boundry.MaxLatitude, 
				MinLongitude = boundry.MinLongitude,
				MaxLongitude = boundry.MaxLongitude 
			};
			
			Measurement[] matches = connection.Query<Measurement>(query, parameters).ToArray();
			
			foreach (var measurement in matches.Where(measurement => Tracker.Validate(measurement.StationNumber, measurement.Date)))
            {
                IsNew = Tracker.Update(measurement.StationNumber, date);
				
                return measurement;
            }
 
            return null;
		}
		
        private static Boundry GetBoundry(decimal latitude, decimal longitude)
        {
            var radius = MileRadius / 69.09M;
 
            return new Boundry 
            { 
                MinLatitude = latitude - radius, 
                MaxLatitude = latitude + radius, 
                MinLongitude = longitude - radius, 
                MaxLongitude = longitude + radius 
            };
        }	
	}
}