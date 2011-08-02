using System;

namespace DataSetExtension
{
	public class Temperature : Td3200
	{
		public string ToString(long sequence)
		{
			var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			var value = ConvertFahrenheitToCelsius(Convert.ToDouble(Value));
			
			return string.Format(
				"{0} 00 {1} {2}", 
				Date.ToString("y M d"), 
				alphabet.Substring((int)sequence, 1), 
				decimal.Truncate(Convert.ToDecimal(value * 10)));
		}
		
		private static double ConvertFahrenheitToCelsius(double f)
		{
	    	return (5.0 / 9.0) * (f - 32);
		}
	}
}