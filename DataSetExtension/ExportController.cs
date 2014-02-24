using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Dapper;
using DataSetExtension.Database;

namespace DataSetExtension
{
    public class ExportController
    {
        private const int GridMin = 1;
        private const int GridMax = 766;
        
        private readonly string query;
        private readonly string basePath;
        private readonly IDbConnection connection;
        private StreamWriter log;

        public ExportController(IDbConnection connection, string path)
        {
            this.connection = connection;
            basePath = path;
            query = CreateQuery();
        }
        
        public void ExportTemperatureMin(int year)
        {
            var measurementTable = MeasurementDatabase.TemperatureMinTable;
            var stationTable = GridStationDatabase.TemperatureMinStationTable;
            var formatter = new TemperatureFormatter();
            
            Export(year, measurementTable, stationTable, formatter, "tmin");
        }

        public void ExportTemperatureMax(int year)
        {
            var measurementTable = MeasurementDatabase.TemperatureMaxTable;
            var stationTable = GridStationDatabase.TemperatureMaxStationTable;
            var formatter = new TemperatureFormatter();
            
            Export(year, measurementTable, stationTable, formatter, "tmax");
        }
        
        public void ExportPrecipitation(int year) 
        {   
            var measurementTable = MeasurementDatabase.PrecipitationTable;
            var stationTable = GridStationDatabase.PrecipitationStationTable;
            var formatter = new PrecipitationFormatter();
            
            Export(year, measurementTable, stationTable, formatter, "prcp");
        }
        
        private void Export(int year, string measurementTable, string stationTable, IFormatter formatter, string directory) 
        {
            using (var addedLog = CreateLogWriter(string.Format("{0}-added-{1}.log", directory, year)))
            {   
                using (log = CreateLogWriter(string.Format("{0}-missing-{1}.log", directory, year)))
                {
                    var tracker = new StationTracker();
                    var locator = new MeasurementLocator(connection, measurementTable, tracker);
                    
                    for (var grid = GridMin; grid <= GridMax; grid++)
                    {
                        using (var stream = new FileStream(GetFile(grid, directory), FileMode.OpenOrCreate, FileAccess.Write)) 
                        {
                            stream.Seek(0, SeekOrigin.End);
                            
                            var stations = GetStations(grid, stationTable);
                            var writer = new MeasurementWriter(stream, stations, year) { Locator = locator, Formatter = formatter };
                            
                            var start = new DateTime(year, 1, 1);
                            var end = GetEndDate(year);
                            var query = string.Format(this.query, stationTable, measurementTable);
                            var measurements = connection.Query<Measurement>(query, new { GridPoint = grid, Start = start, End = end }).ToArray();
            
                            ProcessMeasurements(year, grid, writer, measurements);
                            
                            UpdateStations(writer.GetUpdatedStations(), stationTable, addedLog);
                        }
                    }
                }       
            }   
        }
                                
        private void UpdateStations(GridStation[] stations, string table, StreamWriter log)
        {
            foreach (var station in stations)
            {
                if (station.IsNew) 
                {
                    log.WriteLine(station.GridPoint + " " + station.Number);
                    station.Save(connection, table);
                    
                    continue;
                }
                
                station.Save(connection, table);    
            }
            
            log.Flush();
        }
        
        private string GetFile(int grid, string directory)
        { 
            return Path.Combine(GetDirectory(directory), "gr" + grid.ToString().PadLeft(3, '0'));
        }
        
        private StreamWriter CreateLogWriter(string file)
        {
            return new StreamWriter(File.Create(Path.Combine(basePath, file)));
        }

        private void ProcessMeasurements(int year, int grid, MeasurementWriter writer, IMeasurement[] measurements)
        {
            for (var month = 1; month <= 12; month++)
            {
                var subset = from measurement in measurements
                    where measurement.Date >= new DateTime(year, month, 1) &&
                    measurement.Date <= new DateTime(year, month, DateTime.DaysInMonth(year, month))
                    select measurement;
                    
                writer.Write(subset.ToArray(), month);
            }
            
            if (writer.Missing.Count > 0) 
            {
                LogMissing(grid, writer.Missing);
            }
        }
        
        private void LogMissing(int grid, List<DateTime> missing) 
        {
            log.WriteLine(grid);
            
            foreach (DateTime date in missing) 
            {
                log.WriteLine("    " + date.ToShortDateString());
            }
            
            log.Flush();
        }
        
        private string GetDirectory(string path) 
        {
            var directory = Path.Combine(basePath, path);
            if (!Directory.Exists(directory)) 
            {
                Directory.CreateDirectory(directory);
            }
            
            return directory;
        }
        
        private GridStation[] GetStations(int grid, string table) 
        {
            var query = "select Id, Sequence, GridPoint, GridPointLatitude, GridPointLongitude, Latitude, " + 
                "Longitude, Name, Number, HistoricalRecordCount, RecordCount " + 
                " from " + table + " where GridPoint = @GridPoint";
            
            return connection.Query<GridStation>(query, new { GridPoint = grid }).ToArray();
        }

        private DateTime GetEndDate (int year)
        {
            return new DateTime(year, 12, DateTime.DaysInMonth(year, 12));
        }
        
        static string CreateQuery() 
        {
            var writer = new StringWriter();
            writer.WriteLine("select m.StationId, m.StationNumber, m.Date, m.ObservationHour, m.Value");
            writer.WriteLine("from {0} s inner join {1} m on s.Id = m.StationId");
            writer.WriteLine("where s.GridPoint = @GridPoint and m.Date >= @Start and m.Date <= @End");
            
            return writer.ToString();   
        }
    }
}