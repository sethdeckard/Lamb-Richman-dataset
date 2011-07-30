using System.Data;
using System.Text;
using Dapper;

namespace DataSetExtension
{
    public class Td3200Database
    {
        public const string PrecipitationTd3200Table = "PrecipitationTd3200";
        public const string TemperatureMaxTd3200Table = "TemperatureMaxTd3200";
        public const string TemperatureMinTd3200Table = "TemperatureMinTd3200";

        private readonly IDbConnection connection;

        public Td3200Database(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void CreateTables()
        {
            connection.Execute(GenerateCreateTableStatement(PrecipitationTd3200Table));
            connection.Execute(GenerateCreateTableStatement(TemperatureMaxTd3200Table));
            connection.Execute(GenerateCreateTableStatement(TemperatureMinTd3200Table));
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
            statement.AppendLine("Value INTEGER");
            statement.AppendLine(");");

            return statement.ToString();
        }
    }
}