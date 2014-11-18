using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
    public class FakeMeasurementLocator : IMeasurementLocator
    {
        public bool IsNew { get; set; }
        
        public bool PassNull { get; set; }
        
        public StationTracker Tracker { get; set; }
        
        public Measurement Find(double latitude, double longitude, DateTime date) 
        {
            IsNew = true;
            
            return (PassNull) ? null : CreateMeasurement(); 
        }
        
        private Measurement CreateMeasurement() 
        {
            return new Measurement 
                { 
                    StationNumber = "1", 
                    Station = new Station 
                                { 
                                    Name =  "TestStation", 
                                    State = "ST", 
                                    Latitude = 32.85D,
                                    Longitude = -89.29D
                                } 
                };
        }
    }
}