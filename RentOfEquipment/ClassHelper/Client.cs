using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentOfEquipment.EF
{
    public partial class Client
    {
        public string FIO { get => $"{LastName} {FirstName} {Patronymic}"; }
    }
}
