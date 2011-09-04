using System;
using System.Data;
using Mono.Data.Sqlite;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
	[TestFixture]
	public class DatabaseTest
	{
		[Test]
		public void Compact() 
		{
            using (IDbConnection connection = new SqliteConnection("Data source=:memory:"))
            {
                connection.Open();

                var database = new Database(connection);
				database.Compact();
            }				
		}
	}
}