using System.Data;
using System.IO;
using System.Linq;
using DataSetExtension;
using Mono.Data.Sqlite;
using Dapper;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
	[TestFixture]
	public class CanadaImportTest
	{
		[Test]
		public void Import() 
		{
			var writer = new StreamWriter(new MemoryStream());
			
			writer.WriteLine("3010890200102002-00150 -00105 -00045 -00110 -00140 -00185 -00240 -00240 -00280 -00250 -00235" + 
                " -00200 -00190 -00175 -00175 -00295 -00250 -00230 -00190 -00250 -00185 -00225 -00160 -00200 -00295" + 
                " -00320 -00230 -00085 -99999M-99999M-99999M");
			writer.WriteLine("3012205200212001-00028 -00025 000015 000020 000037 000022 -00024 -00043 -00024 000050 000028 000049" + 
                " 000013 000044 000071 000026 000048 000003 -00016 000032 -00005 -00028 -00112 -00136 -00134 " + 
                "000001 -00013 -00097 -00136 -00136 -00136 ");
			writer.WriteLine("3012208200404012000000 000000 000000 000000 000000 000008 000000 000000 000000 000000 000000T000004" + 
                " 000028 000162 000008 000004 000000 000000 000000T000000 000000 000000T000002 000000 000000 000000" + 
                " 000100 000000T000000 000000 -99999M");
			
			writer.Flush();
			writer.BaseStream.Position = 0;
			
            var temperatureMaxStation = new GridStation { Id = 1, Number = "3012205" }; 
            var precipitationStation = new GridStation { Id = 2, Number = "3012208" };
			var temperatureMinStation = new GridStation { Id = 3, Number = "3010890" };
            using (IDbConnection connection = new SqliteConnection("Data source=:memory:"))
            {
                connection.Open();

                var database = new MeasurementDatabase(connection);
                database.CreateSchema();

                var import = new CanadaImport() 
					{ 
						TemperatureMinStations = new[] { temperatureMinStation }, 
						TemperatureMaxStations = new[] { temperatureMaxStation }, 
						PrecipitationStations = new[] { precipitationStation } 
					};
                import.Import(writer.BaseStream, connection);
				
                var count = connection.Query<long>("select count(*) from TemperatureMin;").First();
                Assert.That(count, Is.EqualTo(28));
				
                count = connection.Query<long>("select count(*) from TemperatureMax;").First();
                Assert.That(count, Is.EqualTo(31));

                count = connection.Query<long>("select count(*) from Precipitation;").First();
                Assert.That(count, Is.EqualTo(30));

				var id = connection.Query<long>("select StationId from TemperatureMax;").First();
                Assert.That(id, Is.EqualTo(1));

				id = connection.Query<long>("select StationId from TemperatureMin;").First();
                Assert.That(id, Is.EqualTo(3));
				
				id = connection.Query<long>("select StationId from Precipitation;").First();
                Assert.That(id, Is.EqualTo(2));
            }
			
		}
	}
}