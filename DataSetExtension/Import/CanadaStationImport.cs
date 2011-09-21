using System;
using System.Data;
using System.IO;

namespace DataSetExtension.Import
{
	public class CanadaStationImport
	{
		public int Total { get; set; }
		
		public void Import(Stream stream, IDbConnection connection)
		{
			var command = Station.CreateCommand(connection);
			
			using (var reader = new StreamReader(stream))
			{				
				while (!reader.EndOfStream) 
				{
					var station = new CanadaStation();
					station.Parse(reader.ReadLine());
					station.Save(connection, command);
						
					Total += 1;
				}
			}
			
			command.Transaction.Commit();
		}
	}
}