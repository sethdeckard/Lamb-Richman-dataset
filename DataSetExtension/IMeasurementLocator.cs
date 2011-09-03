using System;
using System.Data;
using System.Linq;
using Dapper;

namespace DataSetExtension
{
	public interface IMeasurementLocator
	{
		bool IsNew { get; set; }
		
		StationTracker Tracker { get; set; }
		
		Measurement Find(decimal latitude, decimal longitude, DateTime date);	
	}
}