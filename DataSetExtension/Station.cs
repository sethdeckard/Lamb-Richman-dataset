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

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }		
		
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
			
			Latitude = ParseLatitude(record.Substring(149, 9));
			Longitude = ParseLongitude(record.Substring(158, 10));
		}
		
		private decimal ConvertDegreeAngle(decimal degrees, decimal minutes, decimal seconds)
		{
			var sign = Math.Sign(degrees);
			minutes *= sign;
			seconds *= sign;
			
		    return Math.Round(degrees + (decimal)(minutes/60) + (decimal)(seconds/3600), 6);
		}
		
		private decimal ParseLatitude(string value)
		{
			var degrees = decimal.Parse(value.Substring(0, 3));
			var minutes = decimal.Parse(value.Substring(3, 2));
			var seconds = decimal.Parse(value.Substring(6, 2));
			
			return ConvertDegreeAngle(degrees, minutes, seconds);
		}
		
		private decimal ParseLongitude(string longitude)
		{
			var degrees = decimal.Parse(longitude.Substring(0, 4));
			var minutes = decimal.Parse(longitude.Substring(5, 2));
			var seconds = decimal.Parse(longitude.Substring(8, 2));
			
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