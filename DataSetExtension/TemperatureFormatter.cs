using System;

namespace DataSetExtension
{
	public class TemperatureFormatter : IFormatter
	{
		private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		
		public string Format(IMeasurement measurement, long sequence)
		{
			var sequenceCode = "0";
			if (sequence >= 0) 
			{
				sequenceCode = alphabet.Substring((int)sequence, 1);
			}
			
			var celsius = ConvertFahrenheitToCelsius(Convert.ToDouble(measurement.Value));
			var formatted = decimal.Round(Convert.ToDecimal(celsius), 1).ToString("#.0").PadLeft(5, ' ').Replace(".","");
			
			var hour = measurement.ObservationHour.ToString().PadLeft(2, ' ');	
			
			var year = measurement.Date.ToString("yy");
			if (year.StartsWith("0"))
			{
				year = year.Remove(0, 1).PadLeft(2, ' ');	
			}		
			
			var dateFormat = year + 
				measurement.Date.Month.ToString().PadLeft(2, ' ') + 
				measurement.Date.Day.ToString().PadLeft(2, ' ');
			
			return string.Format(
				"{0} {1} {2} {3}",
				dateFormat,
				hour,
				sequenceCode, 
				formatted);
		}
		
		private static double ConvertFahrenheitToCelsius(double f)
		{
	    	return (5.0 / 9.0) * (f - 32);
		}
	}
}