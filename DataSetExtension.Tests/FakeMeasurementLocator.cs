
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
namespace DataSetExtension.Tests
{
	public class FakeMeasurementLocator : IMeasurementLocator
	{
		public Measurement[] Find(decimal latitude, decimal longitude, DateTime date) 
		{
			return new Measurement[] { };
		}
	}
}
