using System;
using System.Data;
using System.Linq;
using Dapper;
namespace DataSetExtension
{
    public class Boundry
    {
        public double MinLatitude { get; set; }
 
        public double MaxLatitude { get; set; }
 
        public double MinLongitude { get; set; }
 
        public double MaxLongitude { get; set; }
    }
}