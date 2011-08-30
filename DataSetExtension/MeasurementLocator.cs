using System;
using System.Data;
using System.Linq;
using Dapper;

namespace DataSetExtension
{
	public class MeasurementLocator<T> where T : IMeasurement
	{
		private readonly string table;
		private readonly IDbConnection connection;
		
		public MeasurementLocator(IDbConnection connection, string table)
		{
			this.connection = connection;
			this.table = table;
		}
		
		public T[] Find(decimal latitude, decimal longitude, DateTime date)
		{
			decimal latitudeMin = 0;
			decimal latitudeMax = 0;
			decimal longitudeMin = 0;
			decimal longitudeMax = 0;
			
			var parameters = new { LatitudeMin = latitudeMin, LatitudeMax = latitudeMax, LongitudeMin = longitudeMin, LongitudeMax = longitudeMax };
			
			return connection.Query<T>("select StationNumber, Date, Value from " + table + 
				" inner join Station s on s.Number = StationNumber " +
				" where Latitude > @LatitudeMin and Latitude < @LatitudeMax", parameters).ToArray();
		}
	}
}