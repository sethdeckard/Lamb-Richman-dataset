using System;
using System.Data;
using System.Linq;
using System.IO;
using DataSetExtension;
using DataSetExtension.Database;
using DataSetExtension.Import;
using Dapper;
using NUnit.Framework;

namespace DataSetExtension.Tests.Import
{
    [TestFixture]
    public class CanadaStationImportTest
    {
        [Test]
        public void Import()
        {
            using (var connection = TestUtility.CreateConnection()) 
            {           
                connection.Open();
                
                var database = new StationDatabase(connection);
                database.CreateSchema();
                
                var writer = new StreamWriter(new MemoryStream());
                writer.WriteLine("3010410\tAurora Lo\t52.65\t115.7166667");
                writer.WriteLine("3010534\tBaseline Lo\t52.13333333\t115.4166667");
                writer.WriteLine("3010712\tBlackstone Lo\t52.78333333\t116.35");
                writer.WriteLine("3010800\tBodo AGDM\t52.11666667\t110.1");
                writer.WriteLine("3010816\tBreton Plots\t53.08333333\t114.4333333");
                writer.WriteLine("3010978\tBusby\t54 \t113.8833333");
                writer.WriteLine("3011350\tCarnwood\t53.23333333\t114.65");
                writer.WriteLine("3011663\tClearwater\t51.98333333\t115.25");
                writer.WriteLine("3011885\tCoronation\t52.08333333\t111.45");
                writer.WriteLine("3011887\tCoronation Climate\t52.06666667\t111.45");
                writer.WriteLine("3011953\tDakota West\t52.75\t113.95");
                writer.WriteLine("3012095\tDonalda South\t52.51666667\t112.35");
                writer.WriteLine("3012116\tDrayton Valley\t53.21666667\t114.95");
                writer.WriteLine("3012206\tEdmonton International\t53.3\t113.6");
                writer.WriteLine("3012209\tEdmonton Municipal\t53.56666667\t113.5166667");
                writer.Flush();
                writer.BaseStream.Position = 0;
                
                var import = new CanadaStationImport();
                import.Import(writer.BaseStream, connection);
                
                var query = connection.Query<CanadaStation>("select * from Station;");
                Assert.That(query.Count(), Is.EqualTo(15));
                Assert.That(import.Total, Is.EqualTo(15));
            }
        }
    }
}