using System;

namespace DataSetExtension
{
	public class PrecipitationFormatter : IFormatter
	{
		public string Format(IMeasurement measurement, long sequence) 
		{
			var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
						
			return string.Format(
				"{0} 0 {1} {2}", 
				measurement.Date.ToString("yyMMdd"), 
				alphabet.Substring((int)sequence, 1), measurement.Value.ToString().PadLeft(3, ' '));
		}
	}

	public interface IFormatter 
	{
		string Format(IMeasurement measurement, long sequence);	
	}
}