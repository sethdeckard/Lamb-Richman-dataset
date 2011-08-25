using System;

namespace DataSetExtension
{
	public class Station
	{
		public long Id { get; set; }
		
        public string Number { get; set; }

        public string Name { get; set; }
		
		public string County { get; set; }
		
		public string State { get; set; }
		
		public string Country { get; set; }
		
		public long StationId { get; set; }

        public long Sequence { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }		
		
		public DateTime StartDate { get; set;}
		
		public DateTime EndDate { get; set; }
		
		public void Parse(string record) 
		{
			Number = record.Substring(0, 6).Trim();
			Name = record.Substring(99, 31).Trim();
			
			Country = record.Substring(38, 21).Trim();
			State = record.Substring(59, 2).Trim();
			County = record.Substring(62,31).Trim(); 
			
			StartDate = ParseDate(record.Substring(130, 9));
			EndDate = ParseDate(record.Substring(139, 9));
			
			Latitude = ParseDegreeAngle(record.Substring(149, 9));
			Longitude = ParseDegreeAngle(record.Substring(158, 10));
		}
		
		private float ConvertDegreeAngle(int degrees, int minutes, int seconds)
		{
		    return degrees + (minutes/60) + (seconds/3600);
		}
		
		private float ParseDegreeAngle(string value)
		{
			//31 34 00 -085 15 00
				
			var degrees = int.Parse(0, 3);
			var minutes = int.Parse(4, 2);
			var seconds = int.Parse(6, 2);
			
			return ConvertDegreeAngle(degrees, minutes, seconds);
		}
		
		private DateTime ParseDate(string value)
		{
			var year = int.Parse(value.Substring(0, 4));
			var month = int.Parse(value.Substring(4, 2));
			var day = int.Parse(value.Substring(6, 2));
			
			return new DateTime(year, month, day);
		}
	}
}