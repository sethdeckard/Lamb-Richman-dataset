using System;
using System.Data;
using System.Linq;
using Dapper;

namespace DataSetExtension
{
	public interface IMeasurementLocator
	{
		Measurement[] Find(decimal latitude, decimal longitude, DateTime date);	
	}
	
	public class MeasurementLocator : IMeasurementLocator
	{
		private const int MileRadius = 100;
		private readonly IDbConnection connection;
		private readonly string query;
		
		public MeasurementLocator()
		{
		}
		
		public MeasurementLocator(IDbConnection connection, string table)
		{
			this.connection = connection;

			this.query = "select StationNumber, Date, Value from " + table + 
				" inner join Station s on s.Number = StationNumber " +
				" where Latitude > @MinLatitude and Latitude < @MaxLatitude" +
				" and Longitude > @MinLongitude and Longitude < @MaxLongitude;";
		}
		
		public virtual Measurement[] Find(decimal latitude, decimal longitude, DateTime date)
		{
			var boundry = GetBoundry(latitude, longitude);
			
			var parameters = new 
			{ 
				MinLatitude = boundry.MinLatitude, 
				MaxLatitude = boundry.MaxLatitude, 
				MinLongitude = boundry.MinLongitude,
				MaxLongitude = boundry.MaxLongitude 
			};
			
			return connection.Query<Measurement>(query, parameters).ToArray();
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