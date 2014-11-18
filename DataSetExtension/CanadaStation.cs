using System;

namespace DataSetExtension
{
    public class CanadaStation : Station
    {
        public new void Parse(string record)
        {
            var columns = record.Split(new[] { '\t' });
            
            Number = columns[0];
            Name = columns[1];
			Latitude = double.Parse(columns[2]);
			Longitude = double.Parse(columns[3]) * -1;
        }
    }
}