using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Dapper;

namespace DataSetExtension.Import
{
    public class GridStationImport
    {
		public int Total { get; set; }

        public void Import(Stream stream, IDbConnection connection, string table)
        {
			var command = CreateCommand(connection, table);
			
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
						
						((IDataParameter)command.Parameters[":number"]).Value = station.Number;
						((IDataParameter)command.Parameters[":name"]).Value = station.Name;
						((IDataParameter)command.Parameters[":gridpoint"]).Value = station.GridPoint;
						((IDataParameter)command.Parameters[":sequence"]).Value = station.Sequence;
						((IDataParameter)command.Parameters[":latitude"]).Value = station.Latitude;
						((IDataParameter)command.Parameters[":longitude"]).Value = station.Longitude;
						((IDataParameter)command.Parameters[":gridlatitude"]).Value = station.GridPointLatitude;
						((IDataParameter)command.Parameters[":gridlongitude"]).Value = station.GridPointLongitude;
						((IDataParameter)command.Parameters[":count"]).Value = station.HistoricalRecordCount;
						
						command.ExecuteNonQuery();
						
                        Total += 1;
                    }
                }
            }
			
			command.Transaction.Commit();
        }
		
		private IDbCommand CreateCommand(IDbConnection connection, string table)
		{
			var sql = new StringBuilder();
			sql.Append("insert into " + table);
			sql.AppendLine("(Number,Name,GridPoint,Sequence,Latitude,Longitude,GridPointLatitude,GridPointLongitude,HistoricalRecordCount,RecordCount)");
			sql.AppendLine("Values(:number, :name, :gridpoint, :sequence, :latitude, :longitude, :gridlatitude, :gridlongitude, :count, 0);");
			
			var command = connection.CreateCommand();
			command.CommandText = sql.ToString();
			command.Transaction = connection.BeginTransaction();
			
			var idParameter = command.CreateParameter();
			idParameter.ParameterName = ":id";
			command.Parameters.Add(idParameter);
			
			var numberParameter = command.CreateParameter();
			numberParameter.ParameterName = ":number";
			command.Parameters.Add(numberParameter);
			
			var nameParameter = command.CreateParameter();
			nameParameter.ParameterName = ":name";
			command.Parameters.Add(nameParameter);
			
			var gridPointParameter = command.CreateParameter();
			gridPointParameter.ParameterName = ":gridpoint";
			command.Parameters.Add(gridPointParameter);
			
			var sequenceParameter = command.CreateParameter();
			sequenceParameter.ParameterName = ":sequence";
			command.Parameters.Add(sequenceParameter);
			
			var latitudeParameter = command.CreateParameter();
			latitudeParameter.ParameterName = ":latitude";
			command.Parameters.Add(latitudeParameter);
			
			var longitudeParameter = command.CreateParameter();
			longitudeParameter.ParameterName = ":longitude";
			command.Parameters.Add(longitudeParameter);
			
			var gridLatitudeParameter = command.CreateParameter();
			gridLatitudeParameter.ParameterName = ":gridlatitude";
			command.Parameters.Add(gridLatitudeParameter);
			
			var gridLongitudeParameter = command.CreateParameter();
			gridLongitudeParameter.ParameterName = ":gridlongitude";
			command.Parameters.Add(gridLongitudeParameter);
			
			var countParameter = command.CreateParameter();
			countParameter.ParameterName = ":count";
			command.Parameters.Add(countParameter);
			
			return command;
		}
    }
}