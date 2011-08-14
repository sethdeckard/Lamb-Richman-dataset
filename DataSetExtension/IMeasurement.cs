using System;

namespace DataSetExtension
{
	public interface IMeasurement
	{
		long StationId { get; set; }
		
		long Date { get; set; }
		
		DateTime DateTime { get; set; }

        long Value { get; set; }

        string ToString(long sequence);
	}
}