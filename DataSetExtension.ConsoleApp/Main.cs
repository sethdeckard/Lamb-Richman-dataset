using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Mono.Data.Sqlite;
using DataSetExtension;
using DataSetExtension.Database;
using DataSetExtension.Import;
using Dapper;

namespace DataSetExtension.ConsoleApp
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			new Automation
				{ 
					InputDirectory = "data",
					BackupDirectory = "backups",
					OutputDirectory = "output"
				}.Run();
			
			//Commands.ImportTemperatureMinStations(@"/Users/seth/Documents/LRDataSet/output/tmininfo-2001.txt");
			
			//Commands.ImportStations(@"/Users/seth/Documents/LRDataSet/COOP.TXT.2007may", new DateTime(2002, 1, 1));
			
			//Commands.ImportPrecipitationStations(@"/Users/seth/Documents/LRDataSet/test/prcpinfo-2001.txt");
			//Commands.ImportTemperatureMinStations(@"/Users/seth/Documents/LRDataSet/test/tmininfo-2001.txt");
			//Commands.ImportTemperatureMaxStations(@"/Users/seth/Documents/LRDataSet/test/tmaxinfo-2001.txt");
			
			//Commands.ImportTd3200(@"/Users/seth/Documents/LRDataSet/data/TimeSeries_2002.txt");
			//Commands.ImportCanada(@"/Users/seth/Documents/LRDataSet/canada-data/canada.all", 2002);
			
			//Commands.Export(@"/Users/seth/Documents/LRDataSet/output/2002", 2002);
			//Commands.ExportGridStations(@"/Users/seth/Documents/LRDataSet/test", 2002);
		}
	}
}