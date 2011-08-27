using System;
using System.Data;
using System.IO;
using System.Linq;

namespace DataSetExtension.Import
{
	public class CanadaImport : MeasurementImport
	{
		public override void Import(Stream stream, IDbConnection connection) 
		{
			CreateCommand(connection, MeasurementDatabase.TemperatureMinTable);

            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var records = CanadaMeasurement.Parse(reader.ReadLine());
					if (records.Length == 0) 
					{
						continue;
					}
					
					var element = records.First().Element;
					
                    if (element == Element.TemperatureMax)
                    {
                       ImportTemperatureMax(connection, records);
                    }

                    if (element == Element.TemperatureMin)
                    {
						ImportTemperatureMin(connection, records);
                    }

                    if (element == Element.Precipitation)
                    {
						ImportPrecipitation(connection, records);
                    }
					
					Total += 1;
                }
				
				Command.Transaction.Commit();
            }
		}
	}
}