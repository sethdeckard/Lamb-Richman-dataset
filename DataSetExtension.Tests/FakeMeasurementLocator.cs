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
		
		public Measurement Find(decimal latitude, decimal longitude, DateTime date) 
		{
			IsNew = true;
			
			return (PassNull) ? null : new Measurement { StationNumber = "TestStationNumber" };
		}
	}
}