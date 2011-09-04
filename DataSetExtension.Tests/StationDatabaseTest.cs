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
		[Test]
		public void CreateSchema()
		{
            using (IDbConnection connection = new SqliteConnection("Data source=:memory:"))
            {
                connection.Open();

                var database = new StationDatabase(connection);
                database.CreateSchema();

                connection.Query<GridStation>("select Id, Number, Name, State, County, Latitude, Longitude, Start, End from Station");
            }			
		}
		
		[Test]
		public void UpdateIndex()
		{
            using (IDbConnection connection = new SqliteConnection("Data source=:memory:"))
            {
                connection.Open();

                var database = new StationDatabase(connection);
                database.CreateSchema();

                database.UpdateIndex();
            }			
		}
	}
}