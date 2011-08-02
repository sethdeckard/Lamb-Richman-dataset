using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Dapper;

namespace DataSetExtension
{
    public class StationImport
    {
		private const int batchSize = 200;
		
		private StringBuilder statement;
		
        public List<Station> Imported { get; set; }

        public StationImport()
        {
            Imported = new List<Station>();
        }

        public void Import(Stream stream, IDbConnection connection, string table)
        {
			var count = 0;
			statement = new StringBuilder();
			
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
						
						AppendInsertStatement(table, station);

                        Imported.Add(station);
						
						count += 1;
					
						if (count == batchSize) 
						{
							CommitBatch(connection);
						
							statement = new StringBuilder();
							count = 0;
						}
                    }
                }
				
				if (count > 0) 
				{
					CommitBatch(connection);
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
		
		private void AppendInsertStatement(string table, Station station) 
		{
			statement.Append("insert into " + table);
			statement.AppendLine("(Number,Name,GridPoint,Sequence,Latitude,Longitude,GridPointLatitude,GridPointLongitude,HistoricalRecordCount,RecordCount)");
			statement.AppendFormat(
				"values('{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9});", 
				station.Number, 
				station.Name.Replace("'","''"), 
				station.GridPoint, 
				station.Sequence, 
				station.Latitude, 
				station.Longitude,
				station.GridPointLatitude,
				station.GridPointLongitude,
				station.HistoricalRecordCount,
				station.RecordCount);
			statement.AppendLine();
		}
		
		private void CommitBatch(IDbConnection connection)
		{
			var command = connection.CreateCommand();
        	command.CommandText = statement.ToString();
        	command.Transaction = connection.BeginTransaction();
        	
        	command.ExecuteNonQuery();
        	
        	command.Transaction.Commit();
		}
    }
}