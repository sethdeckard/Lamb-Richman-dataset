using System;
using System.Data;
using System.IO;
using DataSetExtension;

namespace DataSetExtension.Import
{
    public class StationImport
    {
        public DateTime Start { get; set; }
        
        public int Total { get; set; }
        
        public void Import(Stream stream, IDbConnection connection)
        {
            var command = Station.CreateCommand(connection);
            
            using (var reader = new StreamReader(stream))
            {
                for (int i = 0; i < 3; i++) 
                {
                    reader.ReadLine();
                }
                
                while (!reader.EndOfStream) 
                {
                    var station = new Station();
                    station.Parse(reader.ReadLine());
                    
                    if (station.Country == "UNITED STATES" && (Start == DateTime.MinValue || station.Start >= Start))
                    {
                        station.Save(connection, command);
                        
                        Total += 1;
                    }
                }
            }
            
            command.Transaction.Commit();
        }
    }
}