using System;
using System.Collections.Generic;
using System.IO;

namespace DataSetExtension
{
    public class Td3200 : Measurement, IMeasurement
    {
        public static Td3200[] Parse(string record)
        {
            var header = record.Substring(4, 30);
            var station = header.Substring(3, 6);
            var year = int.Parse(header.Substring(17, 4));
            var month = int.Parse(header.Substring(21, 2));

            var list = new List<Td3200>();

            ParseItems(record.Remove(0, 34), station, year, month, list);

            return list.ToArray();
        }
		
		public virtual string ToString(long sequence) 
		{
			return sequence.ToString();
		}

        private static void ParseItems(string records, string station, int year, int month, List<Td3200> list)
        {
            var buffer = new char[12];
            var reader = new StringReader(records);
            while (reader.ReadBlock(buffer, 0, 12) > 0) 
            {
                var record = new string(buffer);

                var flag1 = record.Substring(10, 1);
                if (flag1 != " " && flag1 != "J" && flag1 != "T")
                {
                    continue;
                }

                var flag2 = record.Substring(11, 1);
                if (flag2 != "1" && flag2 != "0")
                {
                    continue;
                }

                var day = int.Parse(record.Substring(0, 2));
                var date = new DateTime(year, month, day);
				var value = int.Parse(record.Substring(4, 6));
                var result = new Td3200 { StationNumber = station, Date = date, Value = value };
                list.Add(result);
            }
        }
    }
}