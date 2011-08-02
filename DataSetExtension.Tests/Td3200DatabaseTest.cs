using System.Data;
using Mono.Data.Sqlite;
using DataSetExtension;
using Dapper;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
    [TestFixture]
    public class Td3200DatabaseTest
    {
        [Test]
        public void CreateDatabase()
        {
            using (IDbConnection connection = new SqliteConnection("Data source=:memory:"))
            {
                connection.Open();

                var database = new Td3200Database(connection);
                database.CreateSchema();

                connection.Query<Td3200>("select Id, StationId, StationNumber, Date, DateString, Value from TemperatureMax");

                connection.Query<Td3200>("select Id, StationId, StationNumber, Date, DateString, Value from TemperatureMin");

                connection.Query<Td3200>("select Id, StationId, StationNumber, Date, DateString, Value from Precipitation");
            }
        }
    }
}