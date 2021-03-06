using System.Data;
using Mono.Data.Sqlite;
using DataSetExtension;
using DataSetExtension.Database;
using Dapper;
using NUnit.Framework;

namespace DataSetExtension.Tests.Database
{
    [TestFixture]
    public class MeasurementDatabaseTest
    {
        [Test]
        public void CreateDatabase()
        {
            using (IDbConnection connection = new SqliteConnection("Data source=:memory:"))
            {
                connection.Open();

                var database = new MeasurementDatabase(connection);
                database.CreateSchema();
				
				database.CreateSchema();

                connection.Query<Td3200>("select Id, StationId, StationNumber, Date, DateString, ObservationHour, Value from TemperatureMax");

                connection.Query<Td3200>("select Id, StationId, StationNumber, Date, DateString, ObservationHour, Value from TemperatureMin");

                connection.Query<Td3200>("select Id, StationId, StationNumber, Date, DateString, ObservationHour, Value from Precipitation");
            }
        }
		
        [Test]
        public void UpdateIndex()
        {
            using (IDbConnection connection = new SqliteConnection("Data source=:memory:"))
            {
                connection.Open();

                var database = new MeasurementDatabase(connection);
                database.CreateSchema();

                database.UpdateIndex();
            }
        }
    }
}