using System.Data;
using System.Text;
using Dapper;

namespace DataSetExtension
{
	public class StationDatabase
	{
		public const string TableName = "Station";
		
		private readonly IDbConnection connection;
		
		public StationDatabase(IDbConnection connection)
		{
			this.connection = connection;
		}
		
		public void CreateSchema() 
		{
			connection.Execute(GenerateCreateTableStatement(TableName));			
		}
		
	    private static string GenerateCreateTableStatement(string table)
        {
            var statement = new StringBuilder();
            statement.AppendFormat("CREATE TABLE IF NOT EXISTS {0} (", table);
            statement.AppendLine();
            statement.AppendLine("Id INTEGER PRIMARY KEY AUTOINCREMENT,");
            statement.AppendLine("Number TEXT,");
            statement.AppendLine("Name TEXT,");
			statement.AppendLine("Country TEXT,");
			statement.AppendLine("State TEXT,");
			statement.AppendLine("County TEXT,");
            statement.AppendLine("Latitude DECIMAL,");
            statement.AppendLine("Longitude DECIMAL,");
            statement.AppendLine("Start DATETIME,");
            statement.AppendLine("End DATETIME");
            statement.AppendLine(");");

            return statement.ToString();
        }
	}
}