using System;
using System.Data;
using System.IO;
using System.Linq;
using Mono.Data.Sqlite;
using DataSetExtension.Import;
using Dapper;
using NUnit.Framework;

namespace DataSetExtension.Tests.Import
{
	[TestFixture]
	public class StationImportTest
	{
		[Test]
		public void Import()
		{
			using (var connection = TestUtility.CreateConnection())
			{
				connection.Open();
				
				var database = new StationDatabase(connection);
				database.CreateSchema();
			
				var import = new StationImport();
				
				var stream = GetTestStream();
				import.Import(stream, connection);
				
				var count = connection.Query<long>("select count(*) from Station;").First();

                Assert.That(count, Is.EqualTo(17));
				Assert.That(import.Total, Is.EqualTo(17));
				
				database.UpdateIndex();
			}
		}

		[Test]
		public void ImportWithStartYear()
		{
			using (var connection = CreateConnection())
			{
				connection.Open();
				
				var database = new StationDatabase(connection);
				database.CreateSchema();
			
				var import = new StationImport
				{
					Start = new DateTime(1998, 1, 1)
				};
				
				var stream = GetTestStream();
				import.Import(stream, connection);
				
				var count = connection.Query<long>("select count(*) from Station;").First();

                Assert.That(count, Is.EqualTo(6));
				Assert.That(import.Total, Is.EqualTo(6));
				
				database.UpdateIndex();
			}
		}	
		
		private IDbConnection CreateConnection() 
		{
			var builder = new SqliteConnectionStringBuilder
			{
				DataSource = ":memory:",
				DateTimeFormat = SQLiteDateFormats.Ticks
			};			
			
			return new SqliteConnection(builder.ToString());
		}	
		
		private Stream GetTestStream() 
		{
			var writer = new StreamWriter(new MemoryStream());
			writer.WriteLine("This file was downloaded from NCDC ( ftp://ftp.ncdc.noaa.gov/pub/data/inventories/COOP.TXT.Z ), 29 May 2007");
			writer.WriteLine("");
			writer.WriteLine("COOP      WBAN  WMO   FAA  NWS   ICAO COUNTRY              ST COUNTY                         TIME  COOP STN NAME                  BEGINS   ENDS      LATITUDE LONGITUDE   ELEV");
			writer.WriteLine("010008 02                             UNITED STATES        AL HENRY                          +6    ABBEVILLE                      19511001 19540701  31 34 00 -085 15 00   459  ");
			writer.WriteLine("010008 07                             UNITED STATES        AL HENRY                          +6    ABBEVILLE                      19540701 19561201  31 34 00 -085 15 00   469  ");
			writer.WriteLine("010008 07                             UNITED STATES        AL HENRY                          +6    ABBEVILLE 1 NNW                19561201 19910826  31 35 00 -085 17 00   465  ");
			writer.WriteLine("010008 07                  ABBA1      UNITED STATES        AL HENRY                          +6    ABBEVILLE 1 NNW                19910826 19980501  31 35 00 -085 17 00   465  ");
			writer.WriteLine("010008 07                  ABBA1      UNITED STATES        AL HENRY                          +6    ABBEVILLE 1 NNW                19980501 20010822  31 34 32 -085 15 00   456  ");
			writer.WriteLine("010008 07                  ABBA1      UNITED STATES        AL HENRY                          +6    ABBEVILLE                      20010822 20020801  31 34 19 -085 14 59   456  ");
			writer.WriteLine("010008 07                  ABBA1      UNITED STATES        AL HENRY                          +6    ABBEVILLE                      20020801 99991231  31 34 13 -085 14 54   456  ");
			writer.WriteLine("010063 01                             UNITED STATES        AL WINSTON                        +6    ADDISON                        19480701 19510930  34 13 00 -087 11 00   801  ");
			writer.WriteLine("010063 03                             UNITED STATES        AL WINSTON                        +6    ADDISON                        19511001 19800501  34 13 00 -087 11 00   801  ");
			writer.WriteLine("010063 03                             UNITED STATES        AL WINSTON                        +6    ADDISON                        19800501 19970926  34 13 00 -087 10 00   789  ");
			writer.WriteLine("010063 03                  ADDA1      UNITED STATES        AL WINSTON                        +6    ADDISON                        19970926 20010218  34 12 04 -087 09 28   789  ");
			writer.WriteLine("010063 03                  ADDA1      UNITED STATES        AL WINSTON                        +6    ADDISON                        20010218 20040728  34 12 11 -087 10 53   766  ");
			writer.WriteLine("010063 03                  ADDA1      UNITED STATES        AL WINSTON                        +6    ADDISON                        20040728 20060610  34 12 11 -087 10 53   766  ");
			writer.WriteLine("010063 03                  ADDA1      UNITED STATES        AL WINSTON                        +6    ADDISON                        20060610 20060611  34 12 11 -087 10 53   766  ");
			writer.WriteLine("010071 01                             UNITED STATES        AL LAWRENCE                       +6    ADDISON CNTRL TWR              19480601 19620331  34 25 00 -087 19 00   991  ");
			writer.WriteLine("010112 08                             UNITED STATES        AL MOBILE                         +6    ALABAMA PORT                   19570701 19680228  30 22 00 -088 07 00     7  ");
			writer.WriteLine("010112 08                             UNITED STATES        AL MOBILE                         +6    ALABAMA PORT                   19680228 19680430  30 22 00 -088 07 00     7  ");
			writer.WriteLine("010112 08                             FUCKING SHIT         AL MOBILE                         +6    ALABAMA PORT                   19680228 19680430  30 22 00 -088 07 00     7  ");

			writer.Flush();
			
			writer.BaseStream.Position = 0;
			
			return writer.BaseStream;
		}
	}
}