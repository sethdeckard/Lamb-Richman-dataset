using System;
using System.Collections.Generic;
using System.IO;

namespace DataSetExtension
{
    public class CanadaMeasurement : Measurement, IMeasurement
    {
        public Element Element { get; set; }
 
        public static CanadaMeasurement[] Parse(string line)
        {
            var header = line.Substring(0, 16);
            var number = header.Substring(0, 7);
            var year = int.Parse(header.Substring(7, 4));
            var month = int.Parse(header.Substring(11, 2));
 
            var element = ParseElement(header.Substring(13, 3));
 
            var list = new List<CanadaMeasurement>();
 
            ParseRecords (line, element, list, month, year, number);
 
            return list.ToArray();
        }

        static void ParseRecords(string line, Element element, List<CanadaMeasurement> list, int month, int year, string number)
        {
            var day = 1;
            var reader = new StringReader (line.Remove (0, 16));
            var buffer = new char[7];
            while (reader.ReadBlock(buffer, 0, 7) > 0) {
                var record = new string (buffer);
            
                var flag = record.Substring (6, 1);
                if (flag == " " || flag == "E" || flag == "N" || flag == "Y" || flag == "T") 
                {
                    var value = ParseValue(record.Substring (0, 6));
                    long converted = ConvertValue(value, element, flag == "T");
            
                    var archive = new CanadaMeasurement
                            {
                                StationNumber = number, 
                                Date = new DateTime(year, month, day),
                                Element = element,
                                Value = converted
                            };
            
                    list.Add (archive);
                }
            
                day++;
            }
        }
        
        private static long ConvertValue(double value, Element element, bool trace)
        {
            long converted = 0;
    
            if (element == Element.TemperatureMax || element == Element.TemperatureMin) 
            {
                converted = Convert.ToInt64(ConvertCelsiusToFahrenheit(value));
            }
    
            if (element == Element.Precipitation) 
            {
                var inches = trace ? .01 : value / 25.4;
                converted = Int64.Parse(Math.Round(inches, 2).ToString ().Replace (".", ""));
            }           
            
            return converted;
        }
        
        private static double ParseValue(string value) 
        {
            return double.Parse(value.Insert(value.Length - 1, "."));
        }
        
        private static double ConvertCelsiusToFahrenheit(double value)
        {
            return ((9.0 / 5.0) * value) + 32;
        }
        
        private static Element ParseElement(string value)
        {
            if (value == "001")
            {
                return Element.TemperatureMax; 
            }
 
            if (value == "002")
            {
                return Element.TemperatureMin;  
            }
 
            if (value == "012")
            {
                return Element.Precipitation;  
            }
 
            throw new ArgumentException("Unable to parse value into Element");
        }
        
        public string ToString(long sequence)
        {
            throw new NotImplementedException ();
        }
    }
    
    public enum Element
    {
        TemperatureMin,
        TemperatureMax,
        Precipitation
    }
}