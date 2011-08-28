using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Linq;
using DataSetExtension;
using DataSetExtension.Import;
using Dapper;
using NUnit.Framework;

namespace DataSetExtension.Tests.Import
{
    [TestFixture]
    public class GridStationImportTest
    {
        [Test]
        public void Import()
        {
            var writer = new StreamWriter(new MemoryStream());
            writer.WriteLine("");
            writer.WriteLine("  1 25  81 Flamingo Rn'r Stat, FL         083020 2509  8055   10913");
            writer.WriteLine("           Marathin Shores, FL            085351 2444  8103    4310");
            writer.WriteLine("           Royal Palms Rngr Stat, FL      087760 2523  8036     692");
            writer.WriteLine("                                                     EDITS:       0");
            writer.WriteLine("                                                     TOTAL:   16071");
            writer.WriteLine("");
            writer.WriteLine("734 52 108 Leney, Sask                   4054306 5153 10732   11199");
            writer.WriteLine("           Biggar, Sask                  4040600 5204 10759    4477");
            writer.WriteLine("           Harris, Sask                  4053120 5144 10735     307");
            writer.WriteLine("                                                     EDITS:       0");
            writer.WriteLine("                                                     TOTAL:   16071");
            writer.Flush();

            writer.BaseStream.Position = 0;

            using (IDbConnection connection = TestUtility.CreateConnection())
            {
                connection.Open();

                var database = new GridStationDatabase(connection);
                database.CreateSchema();

                var import = new GridStationImport();
                import.Import(writer.BaseStream, connection, GridStationDatabase.PrecipitationStationTable);
				
                var count = connection.Query<long>("select count(*) from PrecipitationStation;").First();

                Assert.That(count, Is.EqualTo(6));
            }
        }
    }
}