using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using DataSetExtension.Database;

namespace DataSetExtension
{
	public class GridStationExport
	{
		private readonly IDbConnection connection;
		
		public GridStationExport(IDbConnection connection)
		{
			this.connection = connection;
		}
		
		public void ExportTemperatureMax(IGridSummaryWriter writer)
		{
			Export(writer, GridStationDatabase.TemperatureMaxStationTable);
		}
		
		public void ExportTemperatureMin(IGridSummaryWriter writer)
		{
			Export(writer, GridStationDatabase.TemperatureMinStationTable);
		}
		
		public void ExportPrecipitation(IGridSummaryWriter writer) 
		{
			Export(writer, GridStationDatabase.PrecipitationStationTable);
		}
		
		private void Export(IGridSummaryWriter writer, string table)
		{
			var query = BuildQuery(table);
			
			for (long i = 1; i < 766; i++) 
			{
				var stations = connection.Query<GridStation>(query, new { GridPoint = i }).OrderBy(station => station.Sequence);
				writer.Write(stations.ToArray());
			}			
		}
			
		private string BuildQuery(string table)
		{
			var builder = new StringBuilder();
			builder.AppendLine("select Name, Number, GridPoint, GridPointLatitude, GridPointLongitude, ");
			builder.AppendLine("Latitude, Longitude, RecordCount, Sequence");
			builder.AppendLine("from " + table);
			builder.AppendLine("where GridPoint = @GridPoint");
			
			return builder.ToString();
		}
	}
}