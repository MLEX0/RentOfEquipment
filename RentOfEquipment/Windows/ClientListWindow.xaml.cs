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
    /// Логика взаимодействия для ClientListWindow.xaml
    /// </summary>
    public partial class ClientListWindow : Window
    {
        private EF.Employee AuthUser { get; }
        private bool clientChoise = false;

        List<string> listsort = new List<string>()
        {
            "По умолучанию",
            "По фамилии",
            "По имени",
            "По Email",
            "По Дате рождения"
        };

        public ClientListWindow(EF.Employee authUser)
        {
            this.AuthUser = authUser;
            InitializeComponent();
            cmbSort.ItemsSource = listsort;
            cmbSort.SelectedIndex = 0;
            txtAuthUser.Text = $"Вы вошли как: {AuthUser.IdEmployee} {AuthUser.FIO}";
            Filter();
        }

        public ClientListWindow(EF.Employee authUser, bool ClientChoise)
        {
            this.AuthUser = authUser;
            this.clientChoise = ClientChoise;
            InitializeComponent();
            cmbSort.ItemsSource = listsort;
            cmbSort.SelectedIndex = 0;
            txtAuthUser.Text = $"Вы вошли как: {AuthUser.IdEmployee} {AuthUser.FIO}";
            Filter();
        }

        private void Filter() 
        {
            List<EF.Client> listClient = AppData.Context.Client.Where(i => i.isDeleted == false).ToList();

            listClient = listClient
                .Where(i => i.FIO.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim())
                || i.Email.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim())
                || i.Phone.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim()))
                .ToList();


            switch (cmbSort.SelectedIndex)
            {
                case 0:
                    listClient = listClient.OrderBy(i => i.IdClient).ToList();
                    break;

                case 1:
                    listClient = listClient.OrderBy(i => i.LastName).ToList();
                    break;

                case 2:
                    listClient = listClient.OrderBy(i => i.FirstName).ToList();
                    break;

                case 3:
                    listClient = listClient.OrderBy(i => i.Email).ToList();
                    break;

                case 4:
                    listClient = listClient.OrderBy(i => i.Birthday).ToList();
                    break;

                default:
                    listClient = listClient.OrderBy(i => i.IdClient).ToList();
                    break;
            }


            lvEquipment.ItemsSource = listClient;
            tbCountLines.Text = $"Количество строк: {listClient.Count}";

        }


        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            AddEditClientWindow addEditClientWindow = new AddEditClientWindow();
            addEditClientWindow.ShowDialog();
            Filter();
        }
        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void lvEquipment_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Delete) 
            {
                DeleteClient();
            }


            if (e.Key == Key.Enter)
            {
                if (!clientChoise)
                {
                    EditClient();
                }
                else
                {
                    ChoiseClient();
                }
            }

            if (e.Key == Key.Back) 
            {
                EditClient();
            }

        }

        private void DeleteClient() 
        {
            if (AuthUser.Role.IdRole == 1)
            {
                if (lvEquipment.SelectedItem is EF.Client)
                {
                    try
                    {
                        var resClick = MessageBox.Show("Удалить выбранного клиента?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (resClick == MessageBoxResult.No)
                        {
                            return;
                        }


                        var selectedClient = lvEquipment.SelectedItem as EF.Client;
                        selectedClient.isDeleted = true;
                        AppData.Context.SaveChanges();
                        Filter();
                        MessageBox.Show("Клиент успешно удалён!", "Информирование", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void lvEquipment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!clientChoise)
            {
                EditClient();
            }
            else 
            {
                ChoiseClient();
            }
        }

        private void ChoiseClient()
        {
            if (lvEquipment.SelectedItem is EF.Client) 
            {
                var selectedClient = lvEquipment.SelectedItem as EF.Client;
                if (selectedClient != null)
                {
                    AppData.ChoisenClient = selectedClient;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditClient() 
        {
            if (lvEquipment.SelectedItem is EF.Client)
            {
                var selectedClient = lvEquipment.SelectedItem as EF.Client;
                if (selectedClient != null) 
                { 
                    AddEditClientWindow addEditClientWindow = new AddEditClientWindow(selectedClient);
                    addEditClientWindow.ShowDialog();
                    Filter();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
