using System.Data;
using System.Text;
using Dapper;

namespace DataSetExtension
{
    public class Td3200Database
    {
        public const string PrecipitationTable = "Precipitation";
        public const string TemperatureMaxTable = "TemperatureMax";
        public const string TemperatureMinTable = "TemperatureMin";

        private readonly IDbConnection connection;

        public Td3200Database(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void CreateSchema()
        {
            connection.Execute(GenerateCreateTableStatement(PrecipitationTable));
            connection.Execute(GenerateCreateTableStatement(TemperatureMaxTable));
            connection.Execute(GenerateCreateTableStatement(TemperatureMinTable));
        }

        private static string GenerateCreateTableStatement(string table)
        {
            var statement = new StringBuilder();
            statement.AppendFormat("CREATE TABLE IF NOT EXISTS {0} (", table);
            statement.AppendLine();
            statement.AppendLine("Id INTEGER PRIMARY KEY AUTOINCREMENT,");
            statement.AppendLine("StationId INTEGER,");
            statement.AppendLine("StationNumber TEXT,");
            statement.AppendLine("Date INTEGER,");
			statement.AppendLine("DateString TEXT,");
            statement.AppendLine("Value INTEGER");
            statement.AppendLine(");");

            return statement.ToString();
        }
    }
}