using System.Data;
using System.Text;
using Dapper;

namespace DataSetExtension.Database
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
            connection.Execute("CREATE INDEX IF NOT EXISTS \"Index_Station\" ON \"Station\" (\"Number\" ASC, \"Latitude\" ASC, \"Longitude\" ASC, \"Start\" ASC, \"End\" ASC);");
        }
        
        public void UpdateIndex()
        {
            connection.Execute("REINDEX Index_Station;");
        }
        
        private static string GenerateCreateTableStatement(string table)
        {
            var statement = new StringBuilder();
            statement.AppendFormat("CREATE TABLE IF NOT EXISTS {0} (", table);
            statement.AppendLine();
            statement.AppendLine("Id INTEGER PRIMARY KEY AUTOINCREMENT,");
            statement.AppendLine("Number TEXT,");
            statement.AppendLine("Name TEXT,");
            statement.AppendLine("State TEXT,");
            statement.AppendLine("County TEXT,");
            statement.AppendLine("Latitude DOUBLE,");
            statement.AppendLine("Longitude DOUBLE,");
            statement.AppendLine("Start DATETIME,");
            statement.AppendLine("End DATETIME");
            statement.AppendLine(");");

            return statement.ToString();
        }
    }
}