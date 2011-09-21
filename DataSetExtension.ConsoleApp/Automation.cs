using System;
using System.Diagnostics;
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
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			
			for (int i = 2001; i < 2011; i++) 
			{
				Console.WriteLine("Processing " + i);
				
				Commands.ImportCanadaStations(Path.Combine(InputDirectory, "canada-stations.txt"));
				Commands.ImportStations(Path.Combine(InputDirectory, "COOP.TXT.2007may"), new DateTime(i, 1, 1));
				
				var previous = i - 1;
				Commands.ImportPrecipitationStations(Path.Combine(OutputDirectory, "prcpinfo-" + previous + "-historical.txt"));
				Commands.ImportTemperatureMinStations(Path.Combine(OutputDirectory, "tmininfo-" + previous + "-historical.txt"));
				Commands.ImportTemperatureMaxStations(Path.Combine(OutputDirectory, "tmaxinfo-" + previous + "-historical.txt"));
				
				Commands.ImportTd3200(Path.Combine(InputDirectory, "TimeSeries_" + i +".txt"));
				Commands.ImportCanada(Path.Combine(InputDirectory, "canada.all"), i);
				
				Commands.CopyDatabase(BackupDirectory, i + ".sqlite");
			
				Commands.Export(OutputDirectory, i);	
				Commands.ExportGridStations(OutputDirectory, i);
			
				Commands.CopyDatabase(BackupDirectory, i + "-exported.sqlite");
				
				Commands.DeleteDatabase();
				
				Console.WriteLine(i + " complete.");
			}
			
			stopwatch.Stop();
			Console.WriteLine("Total automation run time " + stopwatch.Elapsed.ToString());
		}
	}
}