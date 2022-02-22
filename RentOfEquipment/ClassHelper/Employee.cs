using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentOfEquipment.EF
{
    public partial class Employee
    {
        public string FIO { get => $"{LastName} {FirstName} {Patronymic}"; }
        public string IFO { get => $"{FirstName} {LastName} {Patronymic}"; }
        public string OIF { get => $"{Patronymic} {FirstName} {LastName}"; }
        public string OFI { get => $"{Patronymic} {LastName} {FirstName}"; }
        public string FOI { get => $"{LastName} {Patronymic} {FirstName}"; }
        public string IOF { get => $"{FirstName} {Patronymic} {LastName}"; }
    }
}
