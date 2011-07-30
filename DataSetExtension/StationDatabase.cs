using System.Data;
using System.Text;
using Dapper;

namespace DataSetExtension
{
    public class StationDatabase
    {
        public const string PrecipitationStationTable = "PrecipitationStation";
        public const string TemperatureStationtable = "TemperatureStation";

        private readonly IDbConnection connection;

        public StationDatabase(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void CreateTables()
        {
            connection.Execute(GenerateCreateTableStatement(PrecipitationStationTable));
            connection.Execute(GenerateCreateTableStatement(TemperatureStationtable));
        }

        private static string GenerateCreateTableStatement(string table)
        {
            var statement = new StringBuilder();
            statement.AppendFormat("CREATE TABLE IF NOT EXISTS {0} (", table);
            statement.AppendLine();
            statement.AppendLine("Id INTEGER PRIMARY KEY AUTOINCREMENT,");
            statement.AppendLine("Number TEXT,");
            statement.AppendLine("Name TEXT,");
            statement.AppendLine("GridPoint INTEGER,");
            statement.AppendLine("Sequence INTEGER,");
            statement.AppendLine("Latitude INTEGER,");
            statement.AppendLine("Longitude INTEGER,");
            statement.AppendLine("GridPointLatitude INTEGER,");
            statement.AppendLine("GridPointLongitude INTEGER,");
            statement.AppendLine("HistoricalRecordCount INTEGER,");
            statement.AppendLine("RecordCount INTEGER");
            statement.AppendLine(");");

            return statement.ToString();
        }
    }
}