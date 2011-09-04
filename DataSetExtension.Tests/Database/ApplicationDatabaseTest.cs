using System;
using System.Data;
using Mono.Data.Sqlite;
using DataSetExtension.Database;
using NUnit.Framework;

namespace DataSetExtension.Tests.Database
{
	[TestFixture]
	public class ApplicationDatabaseTest
	{
		[Test]
		public void Compact() 
		{
            using (IDbConnection connection = new SqliteConnection("Data source=:memory:"))
            {
                connection.Open();

                var database = new ApplicationDatabase(connection);
				database.Compact();
            }				
		}
	}
}