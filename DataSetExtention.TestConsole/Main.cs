using System.IO;
using DataSetExtension;

namespace DataSetExtention.TestConsole
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var import = new StationImport();
            import.Import(new FileStream("/Users/seth/Documents/LRDataSet/prcpinfo.txt", FileMode.Open, FileAccess.Read));
		}
	}
}

