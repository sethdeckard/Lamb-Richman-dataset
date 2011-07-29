namespace DataSetExtension
{
    public class Station
    {
        public int Id { get; set; }

        /// <summary>
        /// This is the Station Id (natural key)
        /// </summary>
        public string Number { get; set; }

        public string Name { get; set; }

        public int GridPoint { get; set; }

        /// <summary>
        /// Denotes the sequence of this station in relation to GridPoint (0 = Primary, 1 = secondary, 2...n)
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Latitude of the Station
        /// </summary>
        public int Latitude { get; set; }

        /// <summary>
        /// Longitude of the Station
        /// </summary>
        public int Longitude { get; set; }

        /// <summary>
        /// Rounded Latitude of the Grid Point
        /// </summary>
        public int RoundedLatitude { get; set; }

        /// <summary>
        /// Rounded Longitude of the Grid Point
        /// </summary>
        public int RoundedLongitude { get; set; }

        public int HistoricalRecordCount { get; set; }

        public int RecordCount { get; set; }

        public virtual void Parse(string record)
        {
            var gridpoint = record.Substring(0, 3).Trim();
            if (gridpoint != "")
            {
                GridPoint = int.Parse(record.Substring(0, 3).Trim());
                RoundedLatitude = int.Parse(record.Substring(4, 3).Trim());
                RoundedLongitude = int.Parse(record.Substring(7, 3));
            }

            Name = record.Substring(10, 30).Trim();
            Number = record.Substring(41, 7).Trim();
            Latitude = int.Parse(record.Substring(48, 5).Trim());
            Longitude = int.Parse(record.Substring(53, 6).Trim());
            HistoricalRecordCount = int.Parse(record.Substring(59, 8).Trim());
        }

        public virtual void Parse(string record, Station parent)
        {
            Parse(record);

            GridPoint = parent.GridPoint;
            RoundedLatitude = parent.RoundedLatitude;
            RoundedLongitude = parent.RoundedLongitude;
            Sequence = parent.Sequence + 1;
        }
    }
}