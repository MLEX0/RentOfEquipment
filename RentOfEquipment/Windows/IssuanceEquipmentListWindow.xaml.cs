using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RentOfEquipment.ClassHelper;

namespace RentOfEquipment.Windows
{
    /// <summary>
    /// Логика взаимодействия для IssuanceEquipmentListWindow.xaml
    /// </summary>
    public partial class IssuanceEquipmentListWindow : Window
    {

        private EF.Employee AuthUser { get; }

        List<string> listsort = new List<string>()
        {
            "По умолучанию",
            "По наименованию оборудования",
            "По ФИО клиента",
            "По ФИО сотрудника",
            "По дате сдачи",
            "По дате выдачи"
        };

        public IssuanceEquipmentListWindow(EF.Employee authUser)
        {
            this.AuthUser = authUser;
            InitializeComponent();
           
            cmbSort.ItemsSource = listsort;
            cmbSort.SelectedIndex = 0;

            txtAuthUser.Text = $"Вы вошли как: {AuthUser.IdEmployee} {AuthUser.FIO}";
            Filter();
        }

        private void Filter()
        {
            List<EF.ClientProduct> listClientProduct = AppData.Context.ClientProduct.Where(i => i.IsReturned == false && i.IsDeleted != true).ToList();
            if (cbxReturned.IsChecked.Value) 
            {
                listClientProduct = AppData.Context.ClientProduct.Where(i => i.IsDeleted != true).ToList();
            }



            listClientProduct = listClientProduct
                .Where(i => i.Client.FIO.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim())
                || i.Employee.FIO.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim())
                || i.Product.Name.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim()))
                .ToList();

            switch (cmbSort.SelectedIndex)
            {
                case 0:
                    listClientProduct = listClientProduct.OrderBy(i => i.IdClientProduct).ToList();
                    break;

                case 1:
                    listClientProduct = listClientProduct.OrderBy(i => i.Product.Name).ToList();
                    break;

                case 2:
                    listClientProduct = listClientProduct.OrderBy(i => i.Client.FIO).ToList();
                    break;

                case 3:
                    listClientProduct = listClientProduct.OrderBy(i => i.Employee.FIO).ToList();
                    break;

                case 4:
                    listClientProduct = listClientProduct.OrderBy(i => i.RentEndDate).ToList();
                    break;

                case 5:
                    listClientProduct = listClientProduct.OrderBy(i => i.RentStartDate).ToList();
                    break;

                default:
                    listClientProduct = listClientProduct.OrderBy(i => i.IdClientProduct).ToList();
                    break;
            }

            lvEquipment.ItemsSource = listClientProduct;
            tbCountLines.Text = $"Количество строк: {listClientProduct.Count}";

        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            IssuanceEquipmentWindow issuanceEquipmentWindow = new IssuanceEquipmentWindow(AuthUser);
            this.Opacity = 0.5;
            issuanceEquipmentWindow.ShowDialog();
            this.Opacity = 1.0;
            Filter();
        }

        private void lvEquipment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ReturnEquipment();
        }

        private void ReturnEquipment() 
        {
            if (lvEquipment.SelectedItem is EF.ClientProduct)
            {
                if (!(lvEquipment.SelectedItem as EF.ClientProduct).IsReturned)
                {
                    var resClick = MessageBox.Show("Вы точно хотите отметить оборудование, как возврещённое?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (resClick == MessageBoxResult.Yes)
                    {
                        (lvEquipment.SelectedItem as EF.ClientProduct).IsReturned = true;
                        (lvEquipment.SelectedItem as EF.ClientProduct).RentEndDate = DateTime.Now;
                        (lvEquipment.SelectedItem as EF.ClientProduct).Product.Qty++;
                        AppData.Context.SaveChanges();
                        cbxReturned.IsChecked = true;
                        Filter();
                    }
                }
            }
            else 
            {
                MessageBox.Show("Выберите запись!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lvEquipment_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Delete)
            {
                ClientServiceDelete();
            }


            if (e.Key == Key.Enter)
            {
                ReturnEquipment();
            }

        }

        private void ClientServiceDelete()
        {
            if (AuthUser.Role.IdRole == 1)
            {
                if (lvEquipment.SelectedItem is EF.ClientProduct)
                {
                    if (!(lvEquipment.SelectedItem as EF.ClientProduct).IsDeleted)
                    {
                        var resClick = MessageBox.Show("Вы точно хотите удалить запись?\nЭто действие сможет отменить только администратор БД!", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (resClick == MessageBoxResult.Yes)
                        {
                            (lvEquipment.SelectedItem as EF.ClientProduct).IsDeleted = true;
                            AppData.Context.SaveChanges();
                            Filter();
                            MessageBox.Show("Запись успешно удалена!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Выберите запись!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else 
            {
                MessageBox.Show("Отказано в доступе!\nДля совершения действия нужны права Администратора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cbxReturned_Checked(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void cbxReturned_Unchecked(object sender, RoutedEventArgs e)
        {
            Filter();
        }
    }
}
