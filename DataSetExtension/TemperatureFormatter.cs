using System;

namespace DataSetExtension
{
	public class TemperatureFormatter : IFormatter
	{
		public string Format(IMeasurement measurement, long sequence)
		{
			var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			var celsius = ConvertFahrenheitToCelsius(Convert.ToDouble(measurement.Value));
			
			var formatted = decimal.Round(Convert.ToDecimal(celsius), 1).ToString("#.0").PadLeft(5, ' ').Replace(".","");
			
			return string.Format(
				"{0} 00 {1} {2}", 
				measurement.Date.ToString("yyMMdd"), 
				alphabet.Substring((int)sequence, 1), 
				formatted);
		}
		
		private static double ConvertFahrenheitToCelsius(double f)
		{
	    	return (5.0 / 9.0) * (f - 32);
		}
	}
}