using System;
using System.Data;
using System.Linq;
using Dapper;

namespace DataSetExtension
{
    public class StationAddedEventArgs : EventArgs
    {
        public string StationNumber { get; set; }
    }
}