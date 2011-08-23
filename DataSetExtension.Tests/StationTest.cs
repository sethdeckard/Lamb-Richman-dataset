using System;

namespace DataSetExtension.Tests
{
	public class StationTest
	{
		public void Parse() 
		{
			var line = "010008 07                  ABBA1      UNITED STATES        AL HENRY                          +6    " + 
				"ABBEVILLE 1 NNW                19910826 19980501  31 35 00 -085 17 00   465  ";
			
			var station = new Station();
			station.Parse(line);
			
			
		}
	}
}

