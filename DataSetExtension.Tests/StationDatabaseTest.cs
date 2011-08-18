using System.Data;
using Mono.Data.Sqlite;
using DataSetExtension;
using NUnit.Framework;
using Dapper;

namespace DataSetExtension.Tests
{
    [TestFixture]
    public class StationDatabaseTest
    {
        private const string PrecipitationQuery = "select Id, Number, Name, GridPoint, Sequence, Latitude, Longitude, GridPointLatitude, GridPointLongitude, " + 
            "HistoricalRecordCount, RecordCount from PrecipitationStation";
        private const string TemperatureMinQuery = "select Id, Number, Name, GridPoint, Sequence, Latitude, Longitude, GridPointLatitude, GridPointLongitude, " + 
            "HistoricalRecordCount, RecordCount from TemperatureMinStation";
		private const string TemperatureMaxQuery = "select Id, Number, Name, GridPoint, Sequence, Latitude, Longitude, GridPointLatitude, GridPointLongitude, " + 
            "HistoricalRecordCount, RecordCount from TemperatureMaxStation";
    
        [Test]
        public void CreateDatabase()
        {
            using (IDbConnection connection = new SqliteConnection("Data source=:memory:"))
            {
                connection.Open();

                var database = new StationDatabase(connection);
                database.CreateSchema();

                connection.Query<Station>(PrecipitationQuery);
                connection.Query<Station>(TemperatureMinQuery);			
				connection.Query<Station>(TemperatureMaxQuery);
            }
        }
    }
}