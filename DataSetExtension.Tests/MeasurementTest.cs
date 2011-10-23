using System;
using System.Data;
using System.Linq;
using DataSetExtension.Database;
using Dapper;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
	[TestFixture]
	public class MeasurementTest
	{
		[Test]
		public void SaveTest()
		{
			using (var connection = TestUtility.CreateConnection())
			{
				connection.Open();
				
				var database = new MeasurementDatabase(connection);
				database.CreateSchema();
				
				var measurement = new Measurement 
				{
					Id = 1,
					StationId = 22,
					StationNumber = "1234",
					Date = DateTime.Parse("01/12/2011"),
					ObservationHour = 18,
					Value = 335
				};
				
				measurement.Save(connection, "TemperatureMax");
				
				var saved = connection.Query<Measurement>("select Id, StationId, StationNumber, Date, ObservationHour, Value from TemperatureMax").First();
				
				Assert.That(saved.Id, Is.EqualTo(measurement.Id));
				Assert.That(saved.StationId, Is.EqualTo(measurement.StationId));
				Assert.That(saved.StationNumber, Is.EqualTo(measurement.StationNumber));
				Assert.That(saved.Date, Is.EqualTo(measurement.Date));
				Assert.That(saved.ObservationHour, Is.EqualTo(measurement.ObservationHour));
				Assert.That(saved.Value, Is.EqualTo(measurement.Value));
			}				
		}
		
		[Test]
		public void SaveWithCommandTest()
		{
			using (var connection = TestUtility.CreateConnection())
			{
				connection.Open();
				
				var database = new MeasurementDatabase(connection);
				database.CreateSchema();
				
				var measurement = new Measurement 
				{
					Id = 1,
					StationId = 22,
					StationNumber = "1234",
					Date = DateTime.Parse("01/12/2011"),
					ObservationHour = 19,
					Value = 335
				};
				
				var command = CreateCommand(connection);
				
				measurement.Save(connection, command);
				
				var saved = connection.Query<Measurement>("select Id, StationId, StationNumber, Date, ObservationHour, Value from TemperatureMax").First();
				
				Assert.That(saved.Id, Is.EqualTo(measurement.Id));
				Assert.That(saved.StationId, Is.EqualTo(measurement.StationId));
				Assert.That(saved.StationNumber, Is.EqualTo(measurement.StationNumber));
				Assert.That(saved.Date, Is.EqualTo(measurement.Date));
				Assert.That(saved.ObservationHour, Is.EqualTo(measurement.ObservationHour));
				Assert.That(saved.Value, Is.EqualTo(measurement.Value));
			}				
		}	
		
		private static IDbCommand CreateCommand(IDbConnection connection)
		{
			var command = connection.CreateCommand();
			command.CommandText = "insert into TemperatureMax(StationId,StationNumber,Date,DateString,ObservationHour,Value)" +
				" Values(:id, :number, :date, :dateString, :hour, :value);";
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
	}
}