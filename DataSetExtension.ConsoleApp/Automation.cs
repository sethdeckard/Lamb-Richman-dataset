using System;
using System.IO;

namespace DataSetExtension.ConsoleApp
{
	public class Automation
	{
		public string InputDirectory { get; set; }
		
		public string OutputDirectory { get; set; }
		
		public string BackupDirectory { get; set; }
		
		public void Run() 
		{
			for (int i = 2001; i < 2011; i++) 
			{
				Commands.ImportStations(Path.Combine(InputDirectory, "COOP.TXT.2007may"), new DateTime(i, 1, 1));
				
				var previous = i - 1;
				Commands.ImportPrecipitationStations(Path.Combine(OutputDirectory, "tmininfo-" + previous + ".txt"));
				Commands.ImportTemperatureMinStations(Path.Combine(OutputDirectory, "tmaxinfo-" + previous + ".txt"));
				Commands.ImportTemperatureMaxStations(Path.Combine(OutputDirectory, "prcpinfo-" + previous + ".txt"));
				
				Commands.ImportTd3200(Path.Combine(InputDirectory, "TimeSeries_" + i +".txt"));
				Commands.ImportCanada(Path.Combine(InputDirectory, "canada.all"), i);
				
				var path = Path.Combine(BackupDirectory, i.ToString());
				Commands.CopyDatabase(path);
			
				Commands.Export(OutputDirectory, i);	
				Commands.ExportGridStations(OutputDirectory, i);
							
				path = Path.Combine(BackupDirectory, i.ToString(), "exported");
				Commands.CopyDatabase(path);
				
				Commands.DeleteDatabase();
			}
		}
	}
}