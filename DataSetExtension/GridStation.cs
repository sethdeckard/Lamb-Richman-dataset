using System;
using System.Data;
using Dapper;

namespace DataSetExtension
{
    public class GridStation
    {
        public long Id { get; set; }
		
        public string Number { get; set; }

        public string Name { get; set; }

        public long GridPoint { get; set; }
		
		public long StationId { get; set; }

        public long Sequence { get; set; }

        public long Latitude { get; set; }

        public long Longitude { get; set; }

        public long GridPointLatitude { get; set; }

        public long GridPointLongitude { get; set; }

        public long HistoricalRecordCount { get; set; }

        public long RecordCount { get; set; }
		
		public bool IsNew { get; set; }

        public virtual void Parse(string record)
        {
            var gridpoint = record.Substring(0, 3).Trim();
            if (gridpoint != "")
            {
                GridPoint = long.Parse(record.Substring(0, 3).Trim());
                GridPointLatitude = long.Parse(record.Substring(4, 3).Trim());
                GridPointLongitude = long.Parse(record.Substring(7, 3));
            }

            Name = record.Substring(10, 30).Trim();
            Number = record.Substring(41, 7).Trim();
            Latitude = long.Parse(record.Substring(48, 5).Trim());
            Longitude = long.Parse(record.Substring(53, 6).Trim());
            HistoricalRecordCount = long.Parse(record.Substring(59, 8).Trim());
        }

        public virtual void Parse(string record, GridStation parent)
        {
            Parse(record);

            GridPoint = parent.GridPoint;
            GridPointLatitude = parent.GridPointLatitude;
            GridPointLongitude = parent.GridPointLongitude;
            Sequence = parent.Sequence + 1;
        }
		
		public virtual void Save(IDbConnection connection, string table)
		{
            var query = "insert into " + table +
                        "(Number,Name,GridPoint,Sequence,Latitude,Longitude,GridPointLatitude,GridPointLongitude,HistoricalRecordCount,RecordCount)";
            query += "values(@Number,@Name,@GridPoint,@Sequence,@Latitude,@Longitude,@GridPointLatitude,@GridPointLongitude,@HistoricalRecordCount,@RecordCount)";

            connection.Execute(query, new
                                          {
                                              Number, 
                                              Name, 
                                              GridPoint, 
                                              Sequence, 
                                              Latitude, 
                                              Longitude, 
                                              GridPointLatitude,
                                              GridPointLongitude,
                                              HistoricalRecordCount,
                                              RecordCount
                                          });			
		}
    }
}