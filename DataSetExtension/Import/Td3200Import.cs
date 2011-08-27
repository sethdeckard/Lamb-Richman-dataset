using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;

namespace DataSetExtension.Import
{
    public class Td3200Import : MeasurementImport
    {
        public override void Import(Stream stream, IDbConnection connection)
        {
			CreateCommand(connection);

            using (var reader = new StreamReader(stream, Encoding.ASCII))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.Contains("TMAX"))
                    {
                       ImportTemperatureMax(connection, Td3200.Parse(line));
                    }

                    if (line.Contains("TMIN"))
                    {
						ImportTemperatureMin(connection, Td3200.Parse(line));
                    }

                    if (line.Contains("PRCP"))
                    {
						ImportPrecipitation(connection, Td3200.Parse(line));
                    }
					
					Total += 1;
                }
				
				Command.Transaction.Commit();
            }
        }
    }
}