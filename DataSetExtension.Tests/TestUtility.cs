using System;
using System.Data;
using Mono.Data.Sqlite;

namespace DataSetExtension.Tests
{
	public class TestUtility
	{
		public static IDbConnection CreateConnection() 
		{
			var builder = new SqliteConnectionStringBuilder
			{
				DataSource = ":memory:",
				DateTimeFormat = SQLiteDateFormats.Ticks
			};			
			
			return new SqliteConnection(builder.ToString());
		}
	}
}

