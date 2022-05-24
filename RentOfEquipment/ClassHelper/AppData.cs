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
        public static EF.Client ChoisenClient { get; set; } = null;
        public static List<EF.Product> ListChoisenEquipment { get; set; } = new List<Product>();
    }
}
