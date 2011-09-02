using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExtension
{
    public class StationTracker
    {
        private readonly Dictionary<string, List<DateTime>> used;

		//public event EventHandler<StationAddedEventArgs> StationAdded;
 
        public StationTracker()
        {
            used = new Dictionary<string, List<DateTime>>();
        }
 
        public bool Validate(string number, DateTime date)
        {
            return !used.ContainsKey(number) || 
				(from value in used[number] where value == date select value).Count() == 0;
        }
 
		//return bool if new
        public bool Update(string number, DateTime date)
        {
            if (used.ContainsKey(number))
            {
                used[number].Add(date);
				
				return false;
            }
            else
            {
                //OnAdded(new StationAddedEventArgs { StationNumber = number });
 
                used.Add(number, new List<DateTime> { date });
				
				return true;
            }
        }
 
        /*protected void OnAdded(StationAddedEventArgs e)
        {
            if (StationAdded != null)
            {
                StationAdded(this, e);
            }
        }*/
    }
}