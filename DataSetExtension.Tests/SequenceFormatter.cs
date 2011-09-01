
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
namespace DataSetExtension.Tests
{
	public class SequenceFormatter : IFormatter
	{
		public string Format(IMeasurement measurement, long sequence)
		{
			return sequence.ToString();
		}	
	}
}
