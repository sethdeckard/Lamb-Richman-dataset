using System;

namespace DataSetExtension
{
	public class PrecipitationFormatter : IFormatter
	{
		private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		
		public string Format(IMeasurement measurement, long sequence) 
		{
			var sequenceCode = "0";
			if (sequence >= 0) 
			{
				sequenceCode = alphabet.Substring((int)sequence, 1);
			}
			
			var hour = measurement.ObservationHour.ToString().PadLeft(2, ' ');
		
			return string.Format(
				"{0} {1} {2} {3}", 
				measurement.Date.ToString("yyMMdd"),
				hour,
				sequenceCode, 
				measurement.Value.ToString().PadLeft(3, ' '));
		}
	}

	public interface IFormatter 
	{
		string Format(IMeasurement measurement, long sequence);	
	}
}