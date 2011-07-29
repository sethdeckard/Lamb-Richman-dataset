using System.Collections.Generic;
using System.IO;

namespace DataSetExtension
{
    public class StationImport
    {
        public List<Station> Imported { get; set; }

        public StationImport()
        {
            Imported = new List<Station>();
        }

        public void Import(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                Station previous = null;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == "" || line.Contains("EDITS:") || line.Contains("TOTAL:"))
                    {
                        previous = null;
                    }
                    else
                    {
                        var station = new Station();

                        if (previous == null)
                        {
                            station.Parse(line);
                        }
                        else
                        {
                            station.Parse(line, previous);
                        }

                        previous = station;

                        Imported.Add(station);
                    }
                }
            }
        }
    }
}