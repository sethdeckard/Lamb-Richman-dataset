using System;
using System.Data;

namespace DataSetExtension
{
	public abstract class Measurement
	{
        public long Id { get; set; }

        public long StationId { get; set; }

        public string StationNumber { get; set; }
        
        public DateTime Date { get; set; }

        public long Value { get; set; }
		
		public void Save(IDbConnection connection)
		{
			throw new NotImplementedException();
		}
		
		public void Save(IDbConnection connection, string table)
		{
			throw new NotImplementedException();
		}
	}
}