using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentOfEquipment.EF
{
    public partial class ClientProduct
    {
        public decimal TotalCost 
        {
            get 
            { 
                return CalculationLibrary.ClientServiceClass.TotalCost(RentStartDate, RentEndDate, Product.Cost);
            } 
        }

        public string TextIsReturned
        {
            get 
            {
                if (IsReturned)
                {
                    return "Возвращено";
                }
                else 
                {
                    return "Не возвращено";
                }
            }
        }

    }
}
