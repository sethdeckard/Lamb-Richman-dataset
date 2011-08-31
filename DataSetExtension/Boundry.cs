using System;
using System.Data;
using System.Linq;
using Dapper;
namespace DataSetExtension
{
    public class Boundry
    {
        public decimal MinLatitude { get; set; }
 
        public decimal MaxLatitude { get; set; }
 
        public decimal MinLongitude { get; set; }
 
        public decimal MaxLongitude { get; set; }
    }
}