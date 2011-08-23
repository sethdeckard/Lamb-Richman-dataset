using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Dapper;

namespace DataSetExtension
{
    public class GridStationImport
    {
		private const int batchSize = 200;
		
		private StringBuilder statement;
		
        public List<GridStation> Imported { get; set; }

        public GridStationImport()
        {
            Imported = new List<GridStation>();
        }

        public void Import(Stream stream, IDbConnection connection, string table)
        {
			var count = 0;
			statement = new StringBuilder();
			
            using (var reader = new StreamReader(stream))
            {
                GridStation previous = null;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == "" || line.Contains("EDITS:") || line.Contains("TOTAL:"))
                    {
                        previous = null;
                    }
                    else
                    {
                        var station = new GridStation();

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
		
		private void AppendInsertStatement(string table, GridStation station) 
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