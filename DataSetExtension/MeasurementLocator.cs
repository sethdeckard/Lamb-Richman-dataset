using System;
using System.Data;

namespace DataSetExtension
{
	public class MeasurementLocator
	{
		public MeasurementLocator(IDbConnection connection, string table)
		{
			
		}
		
		public IMeasurement[] Locate(decimal latitude, decimal longitude)
		{
			throw new NotImplementedException();
		}
	}
}