using System;
using System.Collections.Generic;

namespace DataSetExtension.Tests
{
    public class FakeGridSummaryWriter : IGridSummaryWriter
    {
        public FakeGridSummaryWriter()
        {
            Stations = new List<GridStation>(); 
        }
        
        public List<GridStation> Stations { get; set; }
        
        public void Write(GridStation[] details)
        {
            Stations.AddRange(details);
        }
    }
}