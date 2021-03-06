using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using DataSetExtension;
using DataSetExtension.Database;
using DataSetExtension.Import;
using Mono.Data.Sqlite;
using Dapper;
using NUnit.Framework;

namespace DataSetExtension.Tests.Import
{
    [TestFixture]
    public class Td3200ImportTest
    {
        [Test]
        public void Import()
        {
            var writer = new StreamWriter(new MemoryStream(), Encoding.ASCII);
            writer.WriteLine("0370DLY07145805PRCPHI19890399990310108 00000 R0208 00000 R0308 00000 R0408 00001 00508 00000 R0608 00000 R0708 00000 R0808 00000 R0908 00000 R1008 00000 R1108 00000 R1208 00000 R1308 00000 R1408 00000 R1508 00000 R1608 00000 R1708 00000 R1808 00000 R1908 00000 R2008 00008 02108 00003 02208 00000 R2308 00000 R2408 00000 R2508 00000 R2608 00000 R2708 00000 R2808 00000 R2908 00000 R3008 00007 03108 00000 R");
            writer.WriteLine("0370DLY05145805SNOWTI19890399990310108 00000 R0208 00000 R0308 00000 R0408 00005 00508 00000 R0608 00000 R0708 00000 R0808 00000 R0908 00000 R1008 00000 R1108 00000 R1208 00000 R1308 00000 R1408 00000 R1508 00000 R1608 00000 R1708 00000 R1808 00000 R1908 00000 R2008 00005 02108 00000T02208 00000 R2308 00000 R2408 00000 R2508 00000 R2608 00000 R2708 00000 R2808 00000 R2908 00000 R3008 00000 R3108 00000 R");
            writer.WriteLine("0370DLY05145805TMAX F19890399990310108 00042 00208 00048 00308 00048 00408 00041 00508 00042 00608 00044 00708 00050 00808 00058 N0908 00058 01008 00064 01108 00068 01208 00068 01308 00065 01408 00062 01508 00050 01608 00055 01708 00061 01808 00057 01908 00062 02008 00061 02108 00035 02208 00051 02308 00058 02408 00063 02508 00062 02608 00063 02708 00063 02808 00057 02908 00064 03008 00063 03108 00064 0");
            writer.WriteLine("0370DLY05146805TMIN F19890399990310108 00014 00208 00019 00308 00022 00408 00015 00508 00002 00608 00005 00708 00020 00808 00028 00908 00026 01008 00029 01108 00027 01208 00025 01308 00021 01408 00013 01508 00015 01608 00022 01708 00024 01808 00025 01908 00025 02008 00022 02108 00016 02208 00019 02308 00023 02408 00025 02508 00030 02608 00029 02708 00030 02808 00025 02908 00025 03008 00020 03108 00022 0");
            writer.WriteLine("0370DLY05145805TOBS F19890399990310108 00025 00208 00027 00308 00033 00408 00025 00508 00017 00608 00020 00708 00030 00808 00037 00908 00042 01008 00040 01108 00044 01208 00042 01308 00031 01408 00038 01508 00031 01608 00032 01708 00049 01808 00035 01908 00037 02008 00030 02108 00026 02208 00037 02308 00038 02408 00038 02508 00037 02608 00037 02708 00035 02808 00041 02908 00045 03008 00038 03108 00038 0");
            writer.WriteLine("0370DLY06145805TMAX F19890399990310108 00042 00208 00048 00308 00048 00408 00041 00508 00042 00608 00044 00708 00050 00808 00058 N0908 00058 01008 00064 01108 00068 01208 00068 01308 00065 01408 00062 01508 00050 01608 00055 01708 00061 01808 00057 01908 00062 02008 00061 02108 00035 02208 00051 02308 00058 02408 00063 02508 00062 02608 00063 02708 00063 02808 00057 02908 00064 03008 00063 03108 00064 0");
            writer.WriteLine("0370DLY06145805PRCPHI19890399990310108 00000 R0208 00000 R0308 00000 R0408 00001 00508 00000 R0608 00000 R0708 00000 R0808 00000 R0908 00000 R1008 00000 R1108 00000 R1208 00000 R1308 00000 R1408 00000 R1508 00000 R1608 00000 R1708 00000 R1808 00000 R1908 00000 R2008 00008 02108 00003 02208 00000 R2308 00000 R2408 00000 R2508 00000 R2608 00000 R2708 00000 R2808 00000 R2908 00000 R3008 00007 03108 00000 R");
            writer.WriteLine("0370DLY08145805TMIN F19890399990310108 00014 00208 00019 00308 00022 00408 00015 00508 00002 00608 00005 00708 00020 00808 00028 00908 00026 01008 00029 01108 00027 01208 00025 01308 00021 01408 00013 01508 00015 01608 00022 01708 00024 01808 00025 01908 00025 02008 00022 02108 00016 02208 00019 02308 00023 02408 00025 02508 00030 02608 00029 02708 00030 02808 00025 02908 00025 03008 00020 03108 00022 0");
            writer.WriteLine("0370DLY08145806TMIN F19990399990310108 00014 00208 00019 00308 00022 00408 00015 00508 00002 00608 00005 00708 00020 00808 00028 00908 00026 01008 00029 01108 00027 01208 00025 01308 00021 01408 00013 01508 00015 01608 00022 01708 00024 01808 00025 01908 00025 02008 00022 02108 00016 02208 00019 02308 00023 02408 00025 02508 00030 02608 00029 02708 00030 02808 00025 02908 00025 03008 00020 03108 00022 0");
            writer.Flush();

            writer.BaseStream.Position = 0;

            var temperatureMaxStation = new GridStation { Id = 1, Number = "051458" }; 
            var precipitationStation = new GridStation { Id = 2, Number = "071458" };
            var temperatureMinStation = new GridStation { Id = 3, Number = "051468" };
            using (IDbConnection connection = TestUtility.CreateConnection())
            {
                connection.Open();

                var database = new MeasurementDatabase(connection);
                database.CreateSchema();

                var import = new Td3200Import() 
                    { 
                        TemperatureMinStations = new[] { temperatureMinStation }, 
                        TemperatureMaxStations = new[] { temperatureMaxStation }, 
                        PrecipitationStations = new[] { precipitationStation },
                        Year = 1989
                    };
                import.Import(writer.BaseStream, connection);
                
                var count = connection.Query<long>("select count(*) from TemperatureMax;").First();
                Assert.That(count, Is.EqualTo(60));
                
                count = connection.Query<long>("select count(*) from TemperatureMin;").First();
                Assert.That(count, Is.EqualTo(62));

                count = connection.Query<long>("select count(*) from Precipitation;").First();
                Assert.That(count, Is.EqualTo(8));

                count = connection.Query<long>("select count(*) from TemperatureMax where StationId = 1;").First();
                Assert.That(count, Is.EqualTo(30));

                count = connection.Query<long>("select count(*) from TemperatureMin where StationId = 3;").First();
                Assert.That(count, Is.EqualTo(31));
                
                count = connection.Query<long>("select count(*) from Precipitation where StationId = 2;").First();
                Assert.That(count, Is.EqualTo(4));
                
                count = connection.Query<long>("select count(*) from TemperatureMin where StationNumber = '081458'").First();
                Assert.That(count, Is.EqualTo(31));

                var instance = connection.Query<Td3200>("select ObservationHour from TemperatureMin where StationNumber = '081458' limit 1;").First();
                Assert.That(instance.ObservationHour, Is.EqualTo(8)); //todo check other properties
                
                database.UpdateIndex();
            }
        }
    }
}
