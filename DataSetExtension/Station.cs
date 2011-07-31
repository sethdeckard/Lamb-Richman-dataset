using System;

namespace DataSetExtension
{
    public class Station
    {
        public long Id { get; set; }

        /// <summary>
        /// This is the Station Id (natural key)
        /// </summary>
        public string Number { get; set; }

        public string Name { get; set; }

        public long GridPoint { get; set; }

        /// <summary>
        /// Denotes the sequence of this station in relation to GridPoint (0 = Primary, 1 = secondary, 2...n)
        /// </summary>
        public long Sequence { get; set; }

        /// <summary>
        /// Latitude of the Station
        /// </summary>
        public long Latitude { get; set; }

        /// <summary>
        /// Longitude of the Station
        /// </summary>
        public long Longitude { get; set; }

        /// <summary>
        /// Rounded Latitude of the Grid Point
        /// </summary>
        public long GridPointLatitude { get; set; }

        /// <summary>
        /// Rounded Longitude of the Grid Point
        /// </summary>
        public long GridPointLongitude { get; set; }

        public long HistoricalRecordCount { get; set; }

        public long RecordCount { get; set; }

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

        public virtual void Parse(string record, Station parent)
        {
            Parse(record);

            GridPoint = parent.GridPoint;
            GridPointLatitude = parent.GridPointLatitude;
            GridPointLongitude = parent.GridPointLongitude;
            Sequence = parent.Sequence + 1;
        }
		
		public Station[] FindAll(string table) 
		{
			//connection.Query<Station>("select Id=@Id,Number=@Number,GridPoint=@GridPoint from " + 
			
			throw new NotImplementedException();
		}
    }
}