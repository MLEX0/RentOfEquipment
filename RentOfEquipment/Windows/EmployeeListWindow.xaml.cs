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
    /// Логика взаимодействия для EmployeeListWindow.xaml
    /// </summary>
    /// 
    public partial class EmployeeListWindow : Window
    {
        private EF.Employee AuthUser { get; }

        List<string> listsort = new List<string>() 
        {
            "По умолучанию",
            "По фамилии",
            "По имени",
            "По Email",
            "По Должности"
        }; 

        public EmployeeListWindow(EF.Employee authUser)
        {
            this.AuthUser = authUser;
            InitializeComponent();
            cmbSort.ItemsSource = listsort;
            cmbSort.SelectedIndex = 0;
            Filter();
        }

        private void Filter() 
        {
            List<EF.Employee> listEmployee = AppData.Context.Employee.Where(i => i.isDeleted == false).ToList();


            listEmployee = listEmployee
                .Where(i => i.FIO.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim()) 
                || i.IFO.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim())
                || i.OIF.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim())
                || i.OFI.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim())
                || i.FOI.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim())
                || i.IOF.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim())
                || i.Login.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim()) 
                || i.Phone.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim()))
                .ToList();


            switch (cmbSort.SelectedIndex) 
            { 
                case 0:
                    listEmployee = listEmployee.OrderBy(i => i.IdEmployee).ToList();
                    break;

                case 1:
                    listEmployee = listEmployee.OrderBy(i => i.LastName).ToList();
                    break;

                case 2:
                    listEmployee = listEmployee.OrderBy(i => i.FirstName).ToList();
                    break;

                case 3:
                    listEmployee = listEmployee.OrderBy(i => i.Email).ToList();
                    break;

                case 4:
                    listEmployee = listEmployee.OrderBy(i => i.IdRole).ToList();
                    break;

                default:
                    listEmployee.OrderBy(i => i.IdEmployee).ToList();
                    break;
            }

            lvEquipment.ItemsSource = listEmployee;
            tbCountLines.Text = $"Количество сотрудников: {listEmployee.Count}";
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEditEmployeeWindow addEditEmployeeWindow = new AddEditEmployeeWindow();
            addEditEmployeeWindow.ShowDialog();
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
            if (e.Key == Key.Delete || e.Key == Key.Back) 
            {
                if (lvEquipment.SelectedItem is EF.Employee)
                {
                    try
                    {
                        var resClick = MessageBox.Show("Удалить выбранного пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (resClick == MessageBoxResult.No)
                        {
                            return;
                        }

                        if (this.AuthUser == lvEquipment.SelectedItem as EF.Employee)
                        {
                            MessageBox.Show("Нельзя удалить текущего пользователя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var selectedEmployee = lvEquipment.SelectedItem as EF.Employee;
                        selectedEmployee.isDeleted = true;
                        AppData.Context.SaveChanges();
                        Filter();
                        MessageBox.Show("Пользователь успешно удалён!", "Информирование", MessageBoxButton.OK, MessageBoxImage.Information);
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

            if (e.Key == Key.Enter) 
            { 
                EditEmployee();
            }
        }

        private void lvEquipment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditEmployee();
        }

        public void EditEmployee() 
        {
            if (lvEquipment.SelectedItem is EF.Employee)
            {
                try
                {
                    var selectedEmployee = lvEquipment.SelectedItem as EF.Employee;

                    AddEditEmployeeWindow addEditEmployeeWindow = new AddEditEmployeeWindow(selectedEmployee);
                    addEditEmployeeWindow.ShowDialog();
                    Filter();

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
    }
}
