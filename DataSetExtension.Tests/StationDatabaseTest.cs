using System.Data;
using System.Data.SQLite;
using DataSetExtension;
using NUnit.Framework;
using DataSetExtension.Dapper;

namespace DataSetExtension.Test
{
    [TestFixture]
    public class StationDatabaseTest
    {
        private const string PrecipitationQuery = "select Id, Number, Name, GridPoint, Sequence, Latitude, Longitude, GridPointLatitude, GridPointLongitude, " + 
            "HistoricalRecordCount, RecordCount from PrecipitationStation";
        private const string TemperatureQuery = "select Id, Number, Name, GridPoint, Sequence, Latitude, Longitude, GridPointLatitude, GridPointLongitude, " + 
            "HistoricalRecordCount, RecordCount from TemperatureStation";
    
        [Test]
        public void CreateDatabase()
        {
            using (IDbConnection connection = new SQLiteConnection("Data source=:memory:"))
            {
                connection.Open();

                var database = new StationDatabase(connection);
                database.CreateTables();

                connection.Query<Station>(PrecipitationQuery);

                connection.Query<Station>(TemperatureQuery);

                Assert.Pass();
            }
        }
    }
}