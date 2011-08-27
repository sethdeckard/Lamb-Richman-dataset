using System;
using System.Data;
using System.IO;
using DataSetExtension;

namespace DataSetExtension.Import
{
	public class StationImport
	{
		public void Import(Stream stream, IDbConnection connection)
		{
			var command = CreateCommand(connection);
			
			using (var reader = new StreamReader(stream))
			{
				for (int i = 0; i < 3; i++) 
				{
					reader.ReadLine();
				}
				
				while (!reader.EndOfStream) 
				{
					var station = new Station();
					station.Parse(reader.ReadLine());
					
					if (station.Country == "UNITED STATES")
					{;
						((IDataParameter)command.Parameters[":number"]).Value = station.Number;
						((IDataParameter)command.Parameters[":name"]).Value = station.Name;
						((IDataParameter)command.Parameters[":state"]).Value = station.State;
						((IDataParameter)command.Parameters[":county"]).Value = station.County;
						((IDataParameter)command.Parameters[":latitude"]).Value = station.Latitude;
						((IDataParameter)command.Parameters[":longitude"]).Value = station.Longitude;
						((IDataParameter)command.Parameters[":start"]).Value = station.Start;
						((IDataParameter)command.Parameters[":end"]).Value = station.End;
						
						command.ExecuteNonQuery();
					}
				}
			}
			
			command.Transaction.Commit();
		}
		
		private IDbCommand CreateCommand(IDbConnection connection)
		{
			var command = connection.CreateCommand();
			var sql = "insert into Station(Number, Name, State, County, Latitude, Longitude, Start, End) " + 
				"Values(:number, :name, :state, :county, :latitude, :longitude, :start, :end);";
			command.CommandText = sql;
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
			
			var stateParameter = command.CreateParameter();
			stateParameter.ParameterName = ":state";
			command.Parameters.Add(stateParameter);
			
			var countyParameter = command.CreateParameter();
			countyParameter.ParameterName = ":county";
			command.Parameters.Add(countyParameter);
			
			var latitudeParameter = command.CreateParameter();
			latitudeParameter.ParameterName = ":latitude";
			command.Parameters.Add(latitudeParameter);
			
			var longitudeParameter = command.CreateParameter();
			longitudeParameter.ParameterName = ":longitude";
			command.Parameters.Add(longitudeParameter);
			
			var startParameter = command.CreateParameter();
			startParameter.ParameterName = ":start";
			command.Parameters.Add(startParameter);
			
			var endParameter = command.CreateParameter();
			endParameter.ParameterName = ":end";
			command.Parameters.Add(endParameter);
			
			return command;
		}
	}
}