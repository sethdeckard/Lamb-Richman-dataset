using System;
using System.Data;
using Dapper;

namespace DataSetExtension
{
	public class Database
	{
		public Database(IDbConnection connection) 
		{
			Connection = connection;
		}
		
		protected IDbConnection Connection { get; set; }
		
		public void Compact()
		{
			Connection.Execute("vacuum;");
		}
	}
}