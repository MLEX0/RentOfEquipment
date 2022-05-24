using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentOfEquipment.EF
{
    public partial class Product
    {
        public string TextName { get => $"Наименовение: {Name}"; }
        public string TextQty { get => $"Шт. на складе: {Qty}"; }
        public string TextDescription { get => $"Описание: {Description}"; }
        public string TextIdProduct { get => $"Номер: {IdProduct}"; }
        public string TextCategory { get => $"Категория: {Category.CategoryName}"; }
        public string TextCost { get => $"Цена: {Cost}"; }
        public string TextWarranty { get => $"Срок гарантии: {Warranty}"; }


    }
}
