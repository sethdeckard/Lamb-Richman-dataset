using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using DataSetExtension.Dapper;

namespace DataSetExtension
{
    public class Td3200Import
    {
        private readonly Station[] temperatureStations;
        private readonly Station[] precipitationStations;

        public Td3200Import(Station[] temperatureStations, Station[] precipitationStations)
        {
            this.temperatureStations = temperatureStations;
            this.precipitationStations = precipitationStations;

            TemperatureMin = new List<Td3200>();
            TemperatureMax = new List<Td3200>();
            Precipitation = new List<Td3200>();
        }

        public List<Td3200> TemperatureMin { get; set; }

        public List<Td3200> TemperatureMax { get; set; }

        public List<Td3200> Precipitation { get; set; }

        public void Import(Stream stream, IDbConnection connection)
        {
            using (var reader = new StreamReader(stream, Encoding.ASCII))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (line.Contains("TMAX"))
                    {
                        var results = (from record in Td3200.Parse(line)
                                    join station in temperatureStations on record.StationNumber equals station.Number
                                    select new {record, station}).ToList();

                        results.ForEach(result => result.record.StationId = result.station.Id);

                        foreach (var result in results)
                        {
                            var record = result.record;
                            var query = "insert into " + Td3200Database.TemperatureMaxTd3200Table + "(StationId,StationNumber,Date,Value)";
                            query += "values(@StationId,@StationNumber,@Date,@Value)";

                            connection.Execute(query, new { record.StationId, record.StationNumber, record.Date, record.Value });
                        }

                        TemperatureMax.AddRange((from result in results select result.record).ToArray());
                    }

                    if (line.Contains("TMIN"))
                    {
                        var results = (from record in Td3200.Parse(line)
                                       join station in temperatureStations on record.StationNumber equals station.Number
                                       select new { record, station }).ToList();

                        results.ForEach(r => r.record.StationId = r.station.Id);

                        foreach (var result in results)
                        {
                            var record = result.record;
                            var query = "insert into " + Td3200Database.TemperatureMinTd3200Table + "(StationId,StationNumber,Date,Value)";
                            query += "values(@StationId,@StationNumber,@Date,@Value)";

                            connection.Execute(query, new { record.StationId, record.StationNumber, record.Date, record.Value });
                        }

                        TemperatureMin.AddRange((from result in results select result.record).ToArray());
                    }

                    if (line.Contains("PRCP"))
                    {
                        var results = (from record in Td3200.Parse(line)
                                       join station in precipitationStations on record.StationNumber equals station.Number
                                       select new { record, station }).ToList();

                        results.ForEach(r => r.record.StationId = r.station.Id);

                        foreach (var result in results)
                        {
                            var record = result.record;
                            var query = "insert into " + Td3200Database.PrecipitationTd3200Table + "(StationId,StationNumber,Date,Value)";
                            query += "values(@StationId,@StationNumber,@Date,@Value)";

                            connection.Execute(query, new { record.StationId, record.StationNumber, record.Date, record.Value });
                        }

                        Precipitation.AddRange((from result in results select result.record).ToArray());
                    }
                }
            }
        }
    }
}