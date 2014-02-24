using System;
using System.Data;

namespace DataSetExtension
{
    public class Station
    {
        const double EarthMileRadius = 3958.761;
                
        readonly Func<double, double, double, double, double> calculateDistance = (lat1, lon1, lat2, lon2) => EarthMileRadius * 2 *
        (
            Math.Asin(
                Math.Min(1,
                    Math.Sqrt(
                        (
                            Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) +
                            Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) *
                            Math.Pow(Math.Sin((DiffRadian(lon1, lon2)) / 2.0), 2.0)
                        )
                   )
               )
           )
         );
 
        public long Id { get; set; }
        
        public string Number { get; set; }

        public string Name { get; set; }
        
        public string County { get; set; }
        
        public string State { get; set; }
        
        public string Country { get; set; }

        public decimal Latitude { get; set; } //todo convert to doubles

        public decimal Longitude { get; set; }
        
        public DateTime Start { get; set;}
        
        public DateTime? End { get; set; }
        
        public void Parse(string record)
        {
            Number = record.Substring(0, 6).Trim();
            Name = record.Substring(99, 31).Trim();
            
            Country = record.Substring(38, 21).Trim();
            State = record.Substring(59, 2).Trim();
            County = record.Substring(62,31).Trim(); 
            
            Start = ParseDate(record.Substring(130, 9));
            End = ParseDate(record.Substring(139, 9));
            
            if (End == new DateTime(9999, 12, 31)) 
            {
                End = null; 
            }
            
            Latitude = ParseLatitude(record.Substring(149, 9));
            Longitude = ParseLongitude(record.Substring(158, 10));
        }
        
        public void Save(IDbConnection connection)
        {
            Save(connection, CreateCommand(connection));
        }
        
        public void Save(IDbConnection connection, IDbCommand command) 
        {
            ((IDataParameter)command.Parameters[":number"]).Value = Number;
            ((IDataParameter)command.Parameters[":name"]).Value = Name;
            ((IDataParameter)command.Parameters[":state"]).Value = State;
            ((IDataParameter)command.Parameters[":county"]).Value = County;
            ((IDataParameter)command.Parameters[":latitude"]).Value = Latitude;
            ((IDataParameter)command.Parameters[":longitude"]).Value = Longitude;
            ((IDataParameter)command.Parameters[":start"]).Value = Start;
            ((IDataParameter)command.Parameters[":end"]).Value = End;
            
            command.ExecuteNonQuery();          
        }

        public double CalculateDistance(double latitude, double longitude)
        {
            return calculateDistance(Convert.ToDouble(Latitude), Convert.ToDouble(Longitude), latitude, longitude);
        }
        
        public long GetLatitudeDegrees() 
        {
            return ConvertDecimalToDegreesMinutes(Latitude);
        }
        
        public long GetLongitudeDegrees()
        {
            return ConvertDecimalToDegreesMinutes(Math.Abs(Longitude));
        }

        internal static IDbCommand CreateCommand(IDbConnection connection)
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
        
        static double ToRadian(double val)
        {
            return val * (Math.PI / 180);
        }
 
        static double DiffRadian(double val1, double val2)
        {
            return ToRadian(val2) - ToRadian(val1);
        }
        
        static long ConvertDecimalToDegreesMinutes(decimal value)
        {
            var degrees = Math.Floor(value);        
            var minutes = ((value - degrees) * 60);
            
            return Convert.ToInt64(degrees * 100 + Math.Floor(minutes));            
        }
        
        static decimal ConvertDegreeAngleToDecimalDegrees(decimal degrees, decimal minutes, decimal seconds)
        {
            var sign = Math.Sign(degrees);
            minutes *= sign;
            seconds *= sign;
            
            return Math.Round(degrees + (decimal)(minutes/60) + (decimal)(seconds/3600), 6);
        }
        
        static decimal ParseLatitude(string value)
        {
            var degrees = decimal.Parse(value.Substring(0, 3));
            var minutes = decimal.Parse(value.Substring(3, 2));
            var seconds = decimal.Parse(value.Substring(6, 2));
            
            return ConvertDegreeAngleToDecimalDegrees(degrees, minutes, seconds);
        }
        
        static decimal ParseLongitude(string longitude)
        {
            var degrees = decimal.Parse(longitude.Substring(0, 4));
            var minutes = decimal.Parse(longitude.Substring(5, 2));
            var seconds = decimal.Parse(longitude.Substring(8, 2));
            
            return ConvertDegreeAngleToDecimalDegrees(degrees, minutes, seconds);
        }
        
        static DateTime ParseDate(string value)
        {
            var year = int.Parse(value.Substring(0, 4));
            var month = int.Parse(value.Substring(4, 2));
            var day = int.Parse(value.Substring(6, 2));
            
            return new DateTime(year, month, day);
        }
    }
}