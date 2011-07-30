using System.Collections.Generic;
using System.Data;
using System.IO;
using Dapper;

namespace DataSetExtension
{
    public class StationImport
    {
        public List<Station> Imported { get; set; }

        public StationImport()
        {
            Imported = new List<Station>();
        }

        public void Import(Stream stream, IDbConnection connection, string table)
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

                        SaveStation(station, table, connection);

                        Imported.Add(station);
                    }
                }
            }
        }

        private static void SaveStation(Station station, string table, IDbConnection connection)
        {
            var query = "insert into " + table +
                        "(Number,Name,GridPoint,Sequence,Latitude,Longitude,GridPointLatitude,GridPointLongitude,HistoricalRecordCount,RecordCount)";
            query += "values(@Number,@Name,@GridPoint,@Sequence,@Latitude,@Longitude,@GridPointLatitude,@GridPointLongitude,@HistoricalRecordCount,@RecordCount)";

            connection.Execute(query, new
                                          {
                                              station.Number, 
                                              station.Name, 
                                              station.GridPoint, 
                                              station.Sequence, 
                                              station.Latitude, 
                                              station.Longitude, 
                                              station.GridPointLatitude,
                                              station.GridPointLongitude,
                                              station.HistoricalRecordCount,
                                              station.RecordCount
                                          });
        }
    }
}