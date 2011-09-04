using System.Data;
using System.Text;
using Dapper;

namespace DataSetExtension.Database
{
    public class GridStationDatabase
    {
        public const string PrecipitationStationTable = "PrecipitationStation";
		public const string TemperatureMinStationTable = "TemperatureMinStation";
		public const string TemperatureMaxStationTable = "TemperatureMaxStation";

        private readonly IDbConnection connection;

        public GridStationDatabase(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void CreateSchema()
        {
            connection.Execute(GenerateCreateTableStatement(PrecipitationStationTable));
			connection.Execute(GenerateCreateIndexStatements(PrecipitationStationTable));
			
            connection.Execute(GenerateCreateTableStatement(TemperatureMaxStationTable));
			connection.Execute(GenerateCreateIndexStatements(TemperatureMaxStationTable));
			
			connection.Execute(GenerateCreateTableStatement(TemperatureMinStationTable));
    		connection.Execute(GenerateCreateIndexStatements(TemperatureMinStationTable));
		}
		
		public void UpdateIndex()
		{
			UpdateIndex(PrecipitationStationTable);
			UpdateIndex(TemperatureMaxStationTable);
			UpdateIndex(TemperatureMinStationTable);
		}
		
		private void UpdateIndex(string table)
		{
			connection.Execute(string.Format("REINDEX Index_{0}_NumberGridPoint;", table));
			connection.Execute(string.Format("REINDEX Index_{0}_Number;", table));
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
		
		private static string GenerateCreateIndexStatements(string table)
		{
			var statement = string.Format("CREATE INDEX \"Index_{0}_NumberGridPoint\" ON \"{0}\" (\"Number\" ASC, \"GridPoint\" ASC);", table);
			return statement + string.Format("CREATE INDEX \"Index_{0}_Number\" ON \"{0}\" (\"Number\" ASC);", table);
		}
    }
}