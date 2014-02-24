using System;
using System.Data;

namespace DataSetExtension
{
	public class Measurement : IMeasurement
	{
    	public long Id { get; set; }

        public long StationId { get; set; } //todo should be gridstationId

        public string StationNumber { get; set; }
		
		public Station Station { get; set; }
        
        public DateTime Date { get; set; }
		
		public long ObservationHour { get; set; }

        public long Value { get; set; }
		
		public void Save(IDbConnection connection, string table)
		{
			var command = CreateCommand(connection);
			command.CommandText = GetCommandText(table);
			
			Save(connection, command);
		}
		
		public void Save(IDbConnection connection, IDbCommand command)
		{
			((IDataParameter)command.Parameters[":id"]).Value = StationId;
			((IDataParameter)command.Parameters[":number"]).Value = StationNumber;
			((IDataParameter)command.Parameters[":date"]).Value = Date;
			((IDataParameter)command.Parameters[":dateString"]).Value = Date.ToShortDateString();
			((IDataParameter)command.Parameters[":hour"]).Value = ObservationHour;
			((IDataParameter)command.Parameters[":value"]).Value = Value;			
			
			command.ExecuteNonQuery();
		}
		
		internal static IDbCommand CreateCommand(IDbConnection connection)
		{
			var command = connection.CreateCommand();
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
			
			var hourParameter = command.CreateParameter();
			hourParameter.ParameterName = ":hour";
			command.Parameters.Add(hourParameter);
			
			var valueParameter = command.CreateParameter();
			valueParameter.ParameterName = ":value";
			command.Parameters.Add(valueParameter);
			
			return command;
		}
		
		protected string GetCommandText(string table) 
		{
			return "insert into " + 
				table + 
				"(StationId,StationNumber,Date,DateString,ObservationHour,Value) " + 
				"Values(:id, :number, :date, :dateString, :hour, :value);";
		}
	}
}