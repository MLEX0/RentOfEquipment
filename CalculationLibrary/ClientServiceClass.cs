using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationLibrary
{
    public  class ClientServiceClass
    {
        public static decimal TotalCost(DateTime start, DateTime? end, decimal cost)
        {
            if (end == null) 
            { 
                return 0;
            }
            TimeSpan time = (end.Value - start);
            int timer = Convert.ToInt32(time.TotalDays);
            decimal result = Math.Round(Convert.ToDecimal((Convert.ToDouble(cost) * 0.05) * (timer + 1)), 2);
            return result;
        }
    }
}
