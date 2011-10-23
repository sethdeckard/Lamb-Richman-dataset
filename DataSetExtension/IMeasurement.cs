using System;

namespace DataSetExtension
{
	public interface IMeasurement
	{
		long StationId { get; set; }
		
		string StationNumber { get; set; }
		
		DateTime Date { get; set; }
		
		long ObservationHour { get; set; }
		
        long Value { get; set; }
	}
}