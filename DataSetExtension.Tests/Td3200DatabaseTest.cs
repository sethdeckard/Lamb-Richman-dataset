using System.Data;
using Mono.Data.Sqlite;
using DataSetExtension;
using Dapper;
using NUnit.Framework;

namespace DataSetExtension.Test
{
    [TestFixture]
    public class Td3200DatabaseTest
    {
        private const string TemperatureMaxQuery = "select Id, StationId, StationNumber, Date, Value from TemperatureMaxTd3200";
        private const string TemperatureMinQuery = "select Id, StationId, StationNumber, Date, Value from TemperatureMinTd3200";
        private const string PrecipitationQuery = "select Id, StationId, StationNumber, Date, Value from PrecipitationTd3200";

        [Test]
        public void CreateDatabase()
        {
            using (IDbConnection connection = new SqliteConnection("Data source=:memory:"))
            {
                connection.Open();

                var database = new Td3200Database(connection);
                database.CreateTables();

                connection.Query<Td3200>(TemperatureMaxQuery);

                connection.Query<Td3200>(TemperatureMinQuery);

                connection.Query<Td3200>(PrecipitationQuery);
            }
        }
    }
}