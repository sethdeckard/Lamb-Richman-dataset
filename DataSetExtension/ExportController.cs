using System.Data;
using System.IO;
using System.Linq;
using Dapper;

namespace DataSetExtension
{
	public class ExportController
	{
        private const int GridMin = 1;
        private const int GridMax = 766;

        private readonly string basePath;
        private readonly IDbConnection connection;

        public ExportController(IDbConnection connection, string path)
        {
            this.connection = connection;
            basePath = path;
        }

        public void ExportTemperatureMin(int year)
        {
            for (var grid = GridMin; grid <= GridMax; grid++)
            {
                var query = "select Id, Sequence, GridPoint from " + StationDatabase.TemperatureStationtable + " where GridPoint = @GridPoint";
                var stations = connection.Query<Station>(query, new { GridPoint = grid }).ToArray();

                var path = Path.Combine(basePath, "tmin", "gr" + grid.ToString().PadLeft(3, '0'));
				
				if (!File.Exists(path)) 
				{
					File.Create(path);
				}
				
                var stream = new FileStream(path, FileMode.Append, FileAccess.Write);

                var export = new MeasurementExport(stream, stations, year);

                for (var month = 1; month <= 12; month++)
                {
					var format = "select StationId, Date, Value from {0} inner join {1} on {1}.Id = {0}.StationId where grid = @grid and date > @start and date < @end";
                    query = string.Format(format, Td3200Database.TemperatureMinTable, StationDatabase.TemperatureStationtable);
					
                    var records = connection.Query<Temperature>(query).ToArray();
                    export.Export(records, month);
                }
            }
        }
	}
}

