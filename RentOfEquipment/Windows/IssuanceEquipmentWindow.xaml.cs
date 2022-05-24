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
    /// Логика взаимодействия для IssuanceEquipmentWindow.xaml
    /// </summary>
    public partial class IssuanceEquipmentWindow : Window
    {
        private EF.Employee AuthUser { get; set; }
        private EF.Client ChoiseClient { get; set; }
        private EF.Product EditEquipment { get; set; }
        private EF.ClientProduct AddEquipment { get; set; }

        public IssuanceEquipmentWindow(EF.Employee authUser)
        {
            InitializeComponent();
            AuthUser = authUser;
            AppData.ListChoisenEquipment.Clear();
            lvAddEquipment.ItemsSource = AppData.ListChoisenEquipment.ToList();
            txtEmployee.Text = AuthUser.FIO;
        }

        private void btnChoise_Click(object sender, RoutedEventArgs e)
        {
            ClientListWindow clientListWindow = new ClientListWindow(AuthUser, true);
            clientListWindow.ShowDialog();
            if (AppData.ChoisenClient != null) 
            {
                ChoiseClient = AppData.ChoisenClient;
                txtClient.Text = ChoiseClient.FIO;
            }
            AppData.ChoisenClient = null;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddInList_Click(object sender, RoutedEventArgs e)
        {
            EquipmentListWindow equipmentListWindow = new EquipmentListWindow(AuthUser, true);
            equipmentListWindow.ShowDialog();

            lvAddEquipment.ItemsSource = AppData.ListChoisenEquipment.ToList();
        }

        private void btnDeleteFromList_Click(object sender, RoutedEventArgs e)
        {
            if (lvAddEquipment.SelectedItem is EF.Product) 
            {
                var resClick = MessageBox.Show("Отменить выбор оборудования?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                var removeEquipment = lvAddEquipment.SelectedItem as EF.Product;
                if (removeEquipment != null) 
                { 
                    AppData.ListChoisenEquipment.Remove(removeEquipment);

                    lvAddEquipment.ItemsSource = AppData.ListChoisenEquipment.ToList();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (AppData.ListChoisenEquipment.ToList().Count == 0)
            {
                MessageBox.Show("Оборудование не выбрано!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ChoiseClient == null)
            {
                MessageBox.Show("Клиент не выбран!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (AuthUser == null)
            {
                MessageBox.Show("Сотрудник не выбран!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (dpcStartDate.SelectedDate == null) 
            {
                var result = MessageBox.Show("Дата выдачи не выбрана, сохранить с текущей датой?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No) 
                {
                    return;
                }
                dpcStartDate.SelectedDate = DateTime.Now;
            }

            if (dpcEndDate.SelectedDate == null)
            {
                var result = MessageBox.Show("Дата возврата не выбрана, сохранить без даты возврата?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            if (dpcStartDate.SelectedDate != null) 
            {
                if (dpcEndDate.SelectedDate != null)
                {
                    if (dpcStartDate.SelectedDate.Value > dpcEndDate.SelectedDate.Value)
                    {
                        MessageBox.Show("Дата выдачи превышает дату возврата!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            try
            {
                for (int i = 0; i < AppData.ListChoisenEquipment.ToList().Count; i++)//Уменьшение количества книг
                {
                    EditEquipment = AppData.ListChoisenEquipment[i];

                    EditEquipment.Qty--;

                    AppData.Context.SaveChanges();
                }

                for (int i = 0; i < AppData.ListChoisenEquipment.ToList().Count; i++)
                {
                    AddEquipment = new EF.ClientProduct();
                    AddEquipment.IdProduct = AppData.ListChoisenEquipment[i].IdProduct;
                    AddEquipment.IdEmployee = AuthUser.IdEmployee;
                    AddEquipment.IdClient = ChoiseClient.IdClient;
                    AddEquipment.RentStartDate = dpcStartDate.SelectedDate.Value;
                    if (dpcEndDate.SelectedDate == null)
                    {
                        AddEquipment.RentEndDate = null;
                    }
                    else 
                    {
                        AddEquipment.RentEndDate = dpcEndDate.SelectedDate.Value;
                    }

                    AppData.Context.ClientProduct.Add(AddEquipment);
                }
                AppData.Context.SaveChanges();
                MessageBox.Show("Оборудование успешно выдано!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Ошибка базы данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            AppData.ListChoisenEquipment.Clear();
            this.Close();
        }
    }
}
