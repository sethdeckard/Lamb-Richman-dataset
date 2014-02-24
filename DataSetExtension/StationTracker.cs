using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExtension
{
    public class StationTracker
    {
        private readonly Dictionary<string, List<DateTime>> used;

        public StationTracker()
        {
            used = new Dictionary<string, List<DateTime>>();
        }
 
        public bool Validate(string number, DateTime date)
        {
            return !used.ContainsKey(number) || 
                (from value in used[number] where value == date select value).Count() == 0;
        }
 
        public bool Update(string number, DateTime date) //todo return void, remove dependencies
        {
            if (used.ContainsKey(number))
            {
                used[number].Add(date);
                
                return false;
            }
            else
            {
                used.Add(number, new List<DateTime> { date });
                
                return true;
            }
        }
    }
}