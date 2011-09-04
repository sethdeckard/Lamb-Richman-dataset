using System.Data;
using System.Text;
using Dapper;

namespace DataSetExtension
{
    public class MeasurementDatabase
    {
        public const string PrecipitationTable = "Precipitation";
        public const string TemperatureMaxTable = "TemperatureMax";
        public const string TemperatureMinTable = "TemperatureMin";

        private readonly IDbConnection connection;

        public MeasurementDatabase(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void CreateSchema()
        {
            connection.Execute(GenerateCreateTableStatement(PrecipitationTable));
			connection.Execute(GenerateCreateIndexStatements(PrecipitationTable));
			
            connection.Execute(GenerateCreateTableStatement(TemperatureMaxTable));
			connection.Execute(GenerateCreateIndexStatements(TemperatureMaxTable));
			
            connection.Execute(GenerateCreateTableStatement(TemperatureMinTable));
        	connection.Execute(GenerateCreateIndexStatements(TemperatureMinTable));
		}
		
		public void UpdateIndex()
		{
			UpdateIndex(PrecipitationTable);
			UpdateIndex(TemperatureMaxTable);
			UpdateIndex(TemperatureMinTable);
		}
		
		private void UpdateIndex(string table)
		{
			connection.Execute(string.Format("REINDEX Index_{0}_StationNumber;", table));
			connection.Execute(string.Format("REINDEX Index_{0}_StationId;", table));
		}

        private static string GenerateCreateTableStatement(string table)
        {
            var statement = new StringBuilder();
            statement.AppendFormat("CREATE TABLE IF NOT EXISTS {0} (", table);
            statement.AppendLine();
            statement.AppendLine("Id INTEGER PRIMARY KEY AUTOINCREMENT,");
            statement.AppendLine("StationId INTEGER,");
            statement.AppendLine("StationNumber TEXT,");
            statement.AppendLine("Date DATETIME,");
			statement.AppendLine("DateString TEXT,");
            statement.AppendLine("Value INTEGER");
            statement.AppendLine(");");

            return statement.ToString();
        }
		
		private static string GenerateCreateIndexStatements(string table)
		{
			var statement = string.Format("CREATE INDEX \"Index_{0}_StationNumber\" ON \"{0}\" (\"StationNumber\" ASC, \"Date\" ASC);", table);
			statement += string.Format("CREATE INDEX \"Index_{0}_StationId\" ON \"{0}\" (\"StationId\" ASC, \"Date\" ASC);", table);
			return statement;
		}
    }
}