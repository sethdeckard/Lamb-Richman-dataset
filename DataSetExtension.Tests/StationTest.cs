using System;
using System.Data;
using System.Linq;
using DataSetExtension;
using DataSetExtension.Database;
using Dapper;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
    [TestFixture]
    public class StationTest
    {
        [Test]
        public void Parse() 
        {
            var line = "010008 07                  ABBA1      UNITED STATES        AL HENRY                          +6    " + 
                "ABBEVILLE 1 NNW                19910826 19980501  31 35 00 -085 17 00   465  ";
            
            var station = new Station();
            station.Parse(line);
            
            Assert.That(station.Number, Is.EqualTo("010008"));
            Assert.That(station.State, Is.EqualTo("AL"));
            Assert.That(station.Start, Is.EqualTo(DateTime.Parse("08/26/1991")));
            Assert.That(station.End, Is.EqualTo(DateTime.Parse("05/01/1998")));
            Assert.That(station.Name, Is.EqualTo("ABBEVILLE 1 NNW"));
            
            Assert.That(station.County, Is.EqualTo("HENRY"));
            Assert.That(station.Country, Is.EqualTo("UNITED STATES"));
            Assert.That(station.Longitude, Is.EqualTo(-85.283333M));
            Assert.That(station.Latitude, Is.EqualTo(31.583333M));
            
            station = new Station();
            station.Parse("092570 07                  DAWG1      UNITED STATES        GA TERRELL                        +5    " + 
                "DAWSON                         20021002 99991231  31 46 55 -084 26 59   355  ");
            
            Assert.That(station.End, Is.Null);
        }
        
        [Test]
        public void Save()
        {
            using (var connection = TestUtility.CreateConnection())
            {
                connection.Open();
                
                var database = new StationDatabase(connection);
                database.CreateSchema();
                
                var station = new Station 
                {
                    Name = "TestName",
                    Number = "1234",
                    State = "OK",
                    County = "CountyTest",
                    Latitude = 34.22D,
                    Longitude = -67.44D
                };
                
                station.Save(connection);
                
                var saved = connection.Query<Station>("select Name, Number, State, County, Latitude, Longitude from Station").First();
                
                Assert.That(saved.Number, Is.EqualTo(station.Number));
            }
        }
        
        [Test]
        public void SaveWithCommand()
        {
            using (var connection = TestUtility.CreateConnection())
            {
                connection.Open();
                
                var database = new StationDatabase(connection);
                database.CreateSchema();
                
                var station = new Station 
                {
                    Name = "TestName",
                    Number = "1234",
                    State = "OK",
                    County = "CountyTest",
                    Latitude = 34.22D,
                    Longitude = -67.44D
                };
                
                station.Save(connection, CreateCommand(connection));
                
                var saved = connection.Query<Station>("select Name, Number, State, County, Latitude, Longitude from Station").First();
                
                Assert.That(saved.Number, Is.EqualTo(station.Number));
            }
        }
        
        [Test]
        public void CalculateDistance()
        {
            var station = new Station { Latitude = 22, Longitude = -82 };
            var distance = station.CalculateDistance(23, -85);
 
            Assert.That(Math.Round(distance), Is.EqualTo(204));
        }
        
        [Test]
        public void GetLatitudeDegrees()
        {
            var station = new Station { Latitude = 34.79D };
            var latitude = station.GetLatitudeDegrees();
            
            Assert.That(latitude, Is.EqualTo(3447));
        }
        
        [Test]
        public void GetLongitudeDegrees() 
        {
            var station = new Station { Longitude = -84.28D };
            
            var longitude = station.GetLongitudeDegrees();
            
            Assert.That(longitude, Is.EqualTo(8416));           
        }
        
        private static IDbCommand CreateCommand(IDbConnection connection)
        {
            var command = connection.CreateCommand();
            var sql = "insert into Station(Number, Name, State, County, Latitude, Longitude, Start, End) " + 
                "Values(:number, :name, :state, :county, :latitude, :longitude, :start, :end);";
            command.CommandText = sql;
            command.Transaction = connection.BeginTransaction();
            
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = ":id";
            command.Parameters.Add(idParameter);
            
            var numberParameter = command.CreateParameter();
            numberParameter.ParameterName = ":number";
            command.Parameters.Add(numberParameter);
            
            var nameParameter = command.CreateParameter();
            nameParameter.ParameterName = ":name";
            command.Parameters.Add(nameParameter);
            
            var stateParameter = command.CreateParameter();
            stateParameter.ParameterName = ":state";
            command.Parameters.Add(stateParameter);
            
            var countyParameter = command.CreateParameter();
            countyParameter.ParameterName = ":county";
            command.Parameters.Add(countyParameter);
            
            var latitudeParameter = command.CreateParameter();
            latitudeParameter.ParameterName = ":latitude";
            command.Parameters.Add(latitudeParameter);
            
            var longitudeParameter = command.CreateParameter();
            longitudeParameter.ParameterName = ":longitude";
            command.Parameters.Add(longitudeParameter);
            
            var startParameter = command.CreateParameter();
            startParameter.ParameterName = ":start";
            command.Parameters.Add(startParameter);
            
            var endParameter = command.CreateParameter();
            endParameter.ParameterName = ":end";
            command.Parameters.Add(endParameter);
            
            return command;
        }
    }
}