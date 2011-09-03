using System;
using System.Data;
using System.IO;
using System.Linq;

namespace DataSetExtension.Import
{
	public abstract class MeasurementImport
	{
		protected IDbCommand Command { get; set; }
		
		public int Total { get; set; }
		
		public int Year { get; set; }

        public GridStation[] TemperatureMinStations { get; set; }
		
		public GridStation[] TemperatureMaxStations { get; set; }
		
        public GridStation[] PrecipitationStations { get; set; }
		
		public abstract void Import(Stream stream, IDbConnection connection);
		
		protected void CreateCommand(IDbConnection connection)
		{
			Command = connection.CreateCommand();
			Command.Transaction = connection.BeginTransaction();
			
			var idParameter = Command.CreateParameter();
			idParameter.ParameterName = ":id";
			Command.Parameters.Add(idParameter);
			
			var numberParameter = Command.CreateParameter();
			numberParameter.ParameterName = ":number";
			Command.Parameters.Add(numberParameter);
			
			var dateParameter = Command.CreateParameter();
			dateParameter.ParameterName = ":date";
			Command.Parameters.Add(dateParameter);
			
			var dateString = Command.CreateParameter();
			dateString.ParameterName = ":dateString";
			Command.Parameters.Add(dateString);
			
			var valueParameter = Command.CreateParameter();
			valueParameter.ParameterName = ":value";
			Command.Parameters.Add(valueParameter);
		}

		protected void SetCommandText(string table) 
		{
			Command.CommandText = "insert into " + table + "(StationId,StationNumber,Date,DateString,Value) Values(:id, :number, :date, :dateString, :value);";
		}	
		
		protected void ImportTemperatureMax(IDbConnection connection, IMeasurement[] measurements)
		{
			SetCommandText(MeasurementDatabase.TemperatureMaxTable);
			
			var results = FilterResults(TemperatureMaxStations, measurements);
        	SaveResults(results);
		}
		
		protected void ImportTemperatureMin(IDbConnection connection, IMeasurement[] measurements) 
		{
			SetCommandText(MeasurementDatabase.TemperatureMinTable);
			
			var results = FilterResults(TemperatureMinStations, measurements);
			SaveResults(results);
		}
		
		protected void ImportPrecipitation(IDbConnection connection, IMeasurement[] measurements) 
		{
			SetCommandText(MeasurementDatabase.PrecipitationTable);
			
			var results = FilterResults(PrecipitationStations, measurements);
			SaveResults(results);
		}
		
		private Result[] FilterResults(GridStation[] stations, IMeasurement[] measurements) 
		{
			return (from record in measurements
        	            join gridStation in stations on record.StationNumber equals gridStation.Number into gridStations
						from station in gridStations.DefaultIfEmpty()
						where Year == 0 || record.Date.Year == Year
        	            select new Result { Measurement = record, Station = station}).ToArray();
		}

		private void SaveResults(Result[] results)
		{
			foreach (var result in results)
			{
				result.Measurement.StationId = (result.Station != null) ? result.Station.Id : 0;
				((IDataParameter)Command.Parameters[":id"]).Value = result.Measurement.StationId;
				((IDataParameter)Command.Parameters[":number"]).Value = result.Measurement.StationNumber;
				((IDataParameter)Command.Parameters[":date"]).Value = result.Measurement.Date;
				((IDataParameter)Command.Parameters[":dateString"]).Value = result.Measurement.Date.ToShortDateString();
				((IDataParameter)Command.Parameters[":value"]).Value = result.Measurement.Value;
				
				Command.ExecuteNonQuery();
			}
		}
		
		private class Result 
		{
			public IMeasurement Measurement { get; set; }
			
			public GridStation Station { get; set; }
		}
	}
}