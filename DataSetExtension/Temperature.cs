using System;

namespace DataSetExtension
{
	public class Temperature : Td3200
	{
		public override string ToString(long sequence)
		{
			var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			var celsius = ConvertFahrenheitToCelsius(Convert.ToDouble(Value));
			
			var formatted = decimal.Round(Convert.ToDecimal(celsius), 1).ToString("#.0").PadLeft(5, ' ').Replace(".","");
			
			return string.Format(
				"{0} 00 {1} {2}", 
				DateTime.ToString("yyMMdd"), 
				alphabet.Substring((int)sequence, 1), 
				formatted);
		}
		
		private static double ConvertFahrenheitToCelsius(double f)
		{
	    	return (5.0 / 9.0) * (f - 32);
		}
	}
}