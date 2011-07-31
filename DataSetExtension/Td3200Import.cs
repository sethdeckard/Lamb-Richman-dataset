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
		private const int batchSize = 200;
		
        private readonly Station[] temperatureStations;
        private readonly Station[] precipitationStations;
		private StringBuilder statement;

        public Td3200Import(Station[] temperatureStations, Station[] precipitationStations)
        {
            this.temperatureStations = temperatureStations;
            this.precipitationStations = precipitationStations;
        }
		
		public int Year { get; set; }

        public void Import(Stream stream, IDbConnection connection)
        {
			var total = 0;
			var count = 0;
			statement = new StringBuilder();
			
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
					
					count += 1;
					total += 1;
					
					if (count == batchSize) 
					{
						CommitBatch(connection);
						
						statement = new StringBuilder();
						count = 0;
						
						System.Diagnostics.Debug.WriteLine(total + " TD3200 records imported.");
					}
                }
				
				if (count > 0) 
				{
					CommitBatch(connection);
				}
            }
        }
		
		private void ImportTemperatureMax(IDbConnection connection, string line)
		{
			var results = (from record in Td3200.Parse(line)
        	            join station in temperatureStations on record.StationNumber equals station.Number
						where record.Date.Year == Year || Year == 0
        	            select new {record, station}).ToArray();
        	
        	foreach (var result in results)
        	{
				result.record.StationId = result.station.Id;
				AppendInsertStatement(Td3200Database.TemperatureMaxTable, result.record);
        	}
		}
		
		private void ImportTemperatureMin(IDbConnection connection, string line) 
		{
			var results = (from record in Td3200.Parse(line)
                          join station in temperatureStations on record.StationNumber equals station.Number
						  where record.Date.Year == Year || Year == 0
                          select new { record, station }).ToArray();

			foreach(var result in results) 
			{
				result.record.StationId = result.station.Id;
				AppendInsertStatement(Td3200Database.TemperatureMinTable, result.record);
			}
		}
		
		private void ImportPrecipitation(IDbConnection connection, string line) 
		{
			var results = (from record in Td3200.Parse(line)
                           join station in precipitationStations on record.StationNumber equals station.Number
							where record.Date.Year == Year || Year == 0
                           select new { record, station }).ToArray();

			foreach(var result in results) 
			{
				result.record.StationId = result.station.Id;
				AppendInsertStatement(Td3200Database.PrecipitationTable, result.record);
			}		
		}
		
		private void AppendInsertStatement(string table, Td3200 record)  
		{
			statement.AppendLine("insert into " + table + "(StationId,StationNumber,Date,DateString,Value)");
			statement.AppendFormat("values({0},'{1}',{2},'{3}',{4});", record.StationId, record.StationNumber, record.Date.ToFileTime(), record.Date.ToString(), record.Value);
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