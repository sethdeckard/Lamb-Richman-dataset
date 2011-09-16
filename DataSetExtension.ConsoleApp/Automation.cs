using System;

namespace DataSetExtension.ConsoleApp
{
	public class Automation
	{			
		public void Run() 
		{
			for (int i = 2001; i < 2011; i++) 
			{
				Commands.ImportStations(@"/Users/seth/Documents/LRDataSet/COOP.TXT.2007may", new DateTime(i, 1, 1));
				
				var previous = i - 1;
				Commands.ImportPrecipitationStations(@"/Users/seth/Documents/LRDataSet/output/tmininfo-" + previous + ".txt");
				Commands.ImportTemperatureMinStations(@"/Users/seth/Documents/LRDataSet/output/tmaxinfo-" + previous + ".txt");
				Commands.ImportTemperatureMaxStations(@"/Users/seth/Documents/LRDataSet/output/prcpinfo-" + previous + ".txt");
				
				Commands.ImportTd3200(@"/Users/seth/Documents/LRDataSet/data/TimeSeries_" + i +".txt");
				Commands.ImportCanada(@"/Users/seth/Documents/LRDataSet/canada-data/canada.all", i);
				
				Commands.CopyDatabase(@"/Users/seth/Documents/LRDataSet/database-backups/" + i);
			
				Commands.Export(@"/Users/seth/Documents/LRDataSet/output", i);	
				Commands.ExportGridStations(@"/Users/seth/Documents/LRDataSet/output", i);
				
				Commands.CopyDatabase(@"/Users/seth/Documents/LRDataSet/database-backups/exported/" + i);
				
				Commands.DeleteDatabase();
			}
		}
	}
}