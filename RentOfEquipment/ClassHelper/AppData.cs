using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentOfEquipment.EF;

namespace RentOfEquipment.ClassHelper
{
    public class AppData
    {
        public static Entities Context { get; } = new Entities(); 
    }
}
