using System;
using System.IO;
using System.Linq;

namespace DataSetExtension
{
	public class GridSummaryWriter : IGridSummaryWriter
	{
      	private const string HeaderFormat = "{0}{1} {2} {3}{4} {5} {6}{7}";
        private const string LineFormat = "           {0}{1} {2} {3}{4}";
        private const string EditLine = "                                                     EDITS:       0";
        private const string TotalLine = "                                                     TOTAL:";
 
        private readonly StreamWriter writer;
 
        public GridSummaryWriter(Stream stream)
        {
            writer = new StreamWriter(stream); 
        }
 
        public void Write(GridStation[] details)
        {
            writer.WriteLine(string.Empty);
 
            long total = 0;
 
            var header = true;
            foreach (var detail in details.OrderBy(detail => detail.Sequence))
            {
                if (header)
                {
                    var line = string.Format(
                        HeaderFormat,
                        detail.GridPoint.ToString().PadLeft(3, ' '),
                        detail.GridPointLatitude.ToString().PadLeft(3, ' '),
                        detail.GridPointLongitude.ToString().PadLeft(3, ' '),
                        detail.Name.PadRight(30, ' '),
                        detail.Number.PadLeft(7, ' '),
                        detail.Latitude,
                        detail.Longitude.ToString().PadLeft(5, ' '),
                        detail.RecordCount.ToString().PadLeft(8, ' '));
                    writer.WriteLine(line);
 
                    header = false;
                }
                else
                {
                    var line = string.Format(
                       LineFormat,
                        detail.Name.PadRight(30, ' '),
                        detail.Number.PadLeft(7, ' '),
                        detail.Latitude,
                        detail.Longitude.ToString().PadLeft(5, ' '),
                        detail.RecordCount.ToString().PadLeft(8, ' '));
                    writer.WriteLine(line);                   
                }
 
                total += detail.RecordCount;
            }
 
            writer.WriteLine(TotalLine + total.ToString().PadLeft(8, ' '));
 
            writer.Flush();
        }
	}
}