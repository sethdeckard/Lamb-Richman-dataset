using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;

namespace DataSetExtension
{
    public class Td3200Import
    {
        private readonly Station[] temperatureMinStations;
		private readonly Station[] temperatureMaxStations;
        private readonly Station[] precipitationStations;
		private IDbCommand command;

        public Td3200Import(Station[] temperatureMinStations, Station[] temperatureMaxStations, Station[] precipitationStations)
        {
            this.temperatureMinStations = temperatureMinStations;
			this.temperatureMaxStations = temperatureMaxStations;
            this.precipitationStations = precipitationStations;
        }
		
		public int Total { get; set; }
		
		public int Year { get; set; }

        public void Import(Stream stream, IDbConnection connection)
        {
			CreateCommand(connection, Td3200Database.TemperatureMinTable);

            using (var reader = new StreamReader(stream, Encoding.ASCII))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.Contains("TMAX"))
                    {
                       ImportTemperatureMax(connection, line);
                    }

                    if (line.Contains("TMIN"))
                    {
						ImportTemperatureMin(connection, line);
                    }

                    if (line.Contains("PRCP"))
                    {
						ImportPrecipitation(connection, line);
                    }
					
					Total += 1;
                }
				
				command.Transaction.Commit();
            }
        }
		
		private void ImportTemperatureMax(IDbConnection connection, string line)
		{
			SetCommandText(Td3200Database.TemperatureMaxTable);
			
			var results = (from record in Td3200.Parse(line)
        	            join station in temperatureMaxStations on record.StationNumber equals station.Number
						where record.DateTime.Year == Year || Year == 0
        	            select new {record, station}).ToArray();
        	
        	foreach (var result in results)
        	{
				result.record.StationId = result.station.Id;
				((IDataParameter)command.Parameters[":id"]).Value = result.record.StationId;
				((IDataParameter)command.Parameters[":number"]).Value = result.record.StationNumber;
				((IDataParameter)command.Parameters[":date"]).Value = result.record.Date;
				((IDataParameter)command.Parameters[":dateString"]).Value = result.record.DateTime.ToShortDateString();
				((IDataParameter)command.Parameters[":value"]).Value = result.record.Value;
				
				command.ExecuteNonQuery();
        	}
		}
		
		private void ImportTemperatureMin(IDbConnection connection, string line) 
		{
			SetCommandText(Td3200Database.TemperatureMinTable);
			
			var results = (from record in Td3200.Parse(line)
                          join station in temperatureMinStations on record.StationNumber equals station.Number
						  where record.DateTime.Year == Year || Year == 0
                          select new { record, station }).ToArray();

			foreach(var result in results) 
			{
				result.record.StationId = result.station.Id;
				((IDataParameter)command.Parameters[":id"]).Value = result.record.StationId;
				((IDataParameter)command.Parameters[":number"]).Value = result.record.StationNumber;
				((IDataParameter)command.Parameters[":date"]).Value = result.record.Date;
				((IDataParameter)command.Parameters[":dateString"]).Value = result.record.DateTime.ToShortDateString();
				((IDataParameter)command.Parameters[":value"]).Value = result.record.Value;
				
				command.ExecuteNonQuery();
			}
		}
		
		private void ImportPrecipitation(IDbConnection connection, string line) 
		{
			SetCommandText(Td3200Database.PrecipitationTable);
			
			var results = (from record in Td3200.Parse(line)
                           join station in precipitationStations on record.StationNumber equals station.Number
							where record.DateTime.Year == Year || Year == 0
                           select new { record, station }).ToArray();

			foreach(var result in results) 
			{
				result.record.StationId = result.station.Id;
				((IDataParameter)command.Parameters[":id"]).Value = result.record.StationId;
				((IDataParameter)command.Parameters[":number"]).Value = result.record.StationNumber;
				((IDataParameter)command.Parameters[":date"]).Value = result.record.Date;
				((IDataParameter)command.Parameters[":dateString"]).Value = result.record.DateTime.ToShortDateString();
				((IDataParameter)command.Parameters[":value"]).Value = result.record.Value;
				
				command.ExecuteNonQuery();
			}		
		}
		
		private void CreateCommand(IDbConnection connection, string table)
		{
			command = connection.CreateCommand();
			command.Transaction = connection.BeginTransaction();
			
			var idParameter = command.CreateParameter();
			idParameter.ParameterName = ":id";
			command.Parameters.Add(idParameter);
			
			var numberParameter = command.CreateParameter();
			numberParameter.ParameterName = ":number";
			command.Parameters.Add(numberParameter);
			
			var dateParameter = command.CreateParameter();
			dateParameter.ParameterName = ":date";
			command.Parameters.Add(dateParameter);
			
			var dateString = command.CreateParameter();
			dateString.ParameterName = ":dateString";
			command.Parameters.Add(dateString);
			
			var valueParameter = command.CreateParameter();
			valueParameter.ParameterName = ":value";
			command.Parameters.Add(valueParameter);
		}
		
		private void SetCommandText(string table) 
		{
			command.CommandText = "insert into " + table + "(StationId,StationNumber,Date,DateString,Value) Values(:id, :number, :date, :dateString, :value);";
		}
    }
}