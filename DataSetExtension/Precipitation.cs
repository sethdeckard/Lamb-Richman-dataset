using System;

namespace DataSetExtension
{
	public class Precipitation : Td3200
	{
		public override string ToString(long sequence) 
		{
			var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
						
			return string.Format(
				"{0} 0 {1} {2}", 
				Date.ToString("yyMMdd"), 
				alphabet.Substring((int)sequence, 1), Value.ToString().PadLeft(3, ' '));
		}
	}
}