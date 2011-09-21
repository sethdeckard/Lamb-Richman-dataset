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
				writer.WriteLine("3010410	Aurora Lo	52.65	115.7166667");
				writer.WriteLine("3010534	Baseline Lo	52.13333333	115.4166667");
				writer.WriteLine("3010712	Blackstone Lo	52.78333333	116.35");
				writer.WriteLine("3010800	Bodo AGDM	52.11666667	110.1");
				writer.WriteLine("3010816	Breton Plots	53.08333333	114.4333333");
				writer.WriteLine("3010978	Busby	54	113.8833333");
				writer.WriteLine("3011350	Carnwood	53.23333333	114.65");
				writer.WriteLine("3011663	Clearwater	51.98333333	115.25");
				writer.WriteLine("3011885	Coronation	52.08333333	111.45");
				writer.WriteLine("3011887	Coronation Climate	52.06666667	111.45");
				writer.WriteLine("3011953	Dakota West	52.75	113.95");
				writer.WriteLine("3012095	Donalda South	52.51666667	112.35");
				writer.WriteLine("3012116	Drayton Valley	53.21666667	114.95");
				writer.WriteLine("3012206	Edmonton International	53.3	113.6");
				writer.WriteLine("3012209	Edmonton Municipal	53.56666667	113.5166667");
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