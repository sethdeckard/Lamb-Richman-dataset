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
		
		protected void ImportTemperatureMax(IDbConnection connection, IMeasurement[] records)
		{
			SetCommandText(MeasurementDatabase.TemperatureMaxTable);
			
			var results = (from record in records
        	            join station in TemperatureMaxStations on record.StationNumber equals station.Number
						where Year == 0 || record.DateTime.Year == Year
        	            select new {record, station}).ToArray();
        	
        	foreach (var result in results)
        	{
				result.record.StationId = result.station.Id;
				((IDataParameter)Command.Parameters[":id"]).Value = result.record.StationId;
				((IDataParameter)Command.Parameters[":number"]).Value = result.record.StationNumber;
				((IDataParameter)Command.Parameters[":date"]).Value = result.record.Date;
				((IDataParameter)Command.Parameters[":dateString"]).Value = result.record.DateTime.ToShortDateString();
				((IDataParameter)Command.Parameters[":value"]).Value = result.record.Value;
				
				Command.ExecuteNonQuery();
        	}
		}
		
		protected void ImportTemperatureMin(IDbConnection connection, IMeasurement[] records) 
		{
			SetCommandText(MeasurementDatabase.TemperatureMinTable);
			
			var results = (from record in records
                          join station in TemperatureMinStations on record.StationNumber equals station.Number
						  where Year == 0 || record.DateTime.Year == Year
                          select new { record, station }).ToArray();

			foreach(var result in results) 
			{
				result.record.StationId = result.station.Id;
				((IDataParameter)Command.Parameters[":id"]).Value = result.record.StationId;
				((IDataParameter)Command.Parameters[":number"]).Value = result.record.StationNumber;
				((IDataParameter)Command.Parameters[":date"]).Value = result.record.Date;
				((IDataParameter)Command.Parameters[":dateString"]).Value = result.record.DateTime.ToShortDateString();
				((IDataParameter)Command.Parameters[":value"]).Value = result.record.Value;
				
				Command.ExecuteNonQuery();
			}
		}
		
		protected void ImportPrecipitation(IDbConnection connection, IMeasurement[] records) 
		{
			SetCommandText(MeasurementDatabase.PrecipitationTable);
			
			var results = (from record in records
                           join station in PrecipitationStations on record.StationNumber equals station.Number
						   where Year == 0 || record.DateTime.Year == Year
                           select new { record, station }).ToArray();

			foreach(var result in results) 
			{
				result.record.StationId = result.station.Id;
				((IDataParameter)Command.Parameters[":id"]).Value = result.record.StationId;
				((IDataParameter)Command.Parameters[":number"]).Value = result.record.StationNumber;
				((IDataParameter)Command.Parameters[":date"]).Value = result.record.Date;
				((IDataParameter)Command.Parameters[":dateString"]).Value = result.record.DateTime.ToShortDateString();
				((IDataParameter)Command.Parameters[":value"]).Value = result.record.Value;
				
				Command.ExecuteNonQuery();
			}		
		}
	}
}