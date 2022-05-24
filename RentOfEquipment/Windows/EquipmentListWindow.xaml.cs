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
    /// Логика взаимодействия для EquipmentListWindow.xaml
    /// </summary>
    public partial class EquipmentListWindow : Window
    {
        private EF.Employee AuthUser { get; }
        private bool EquipmentChoise = false;

        List<string> listsort = new List<string>()
        {
            "По умолучанию",
            "По названию",
            "По количеству",
            "По стоимости",
            "По сроку гарантии"
        };

        public EquipmentListWindow(EF.Employee authUser)
        {
            this.AuthUser = authUser;
            InitializeComponent();
            cmbSort.ItemsSource = listsort;
            cmbSort.SelectedIndex = 0;
            Filter();
        }

        public EquipmentListWindow(EF.Employee authUser, bool equipmentChoise)
        {
            EquipmentChoise = equipmentChoise;
            this.AuthUser = authUser;
            InitializeComponent();
            cmbSort.ItemsSource = listsort;
            cmbSort.SelectedIndex = 0;
            Filter();
        }

        private void Filter()
        {
            List<EF.Product> listEmployee = AppData.Context.Product.Where(i => i.isDeleted == false).ToList();

            listEmployee = listEmployee
                .Where(i => i.Name.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim())
                || i.Description.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim())
                || i.Category.CategoryName.ToLower().Trim().Contains(txtSearch.Text.ToLower().Trim()))
                .ToList();

            switch (cmbSort.SelectedIndex)
            {
                case 0:
                    listEmployee = listEmployee.OrderBy(i => i.IdProduct).ToList();
                    break;

                case 1:
                    listEmployee = listEmployee.OrderBy(i => i.Name).ToList();
                    break;

                case 2:
                    listEmployee = listEmployee.OrderBy(i => i.Qty).ToList();
                    break;

                case 3:
                    listEmployee = listEmployee.OrderBy(i => i.Cost).ToList();
                    break;

                case 4:
                    listEmployee = listEmployee.OrderBy(i => i.Warranty).ToList();
                    break;

                default:
                    listEmployee.OrderBy(i => i.IdProduct).ToList();
                    break;
            }
            lvEquipment.ItemsSource = listEmployee;
            tbCountLines.Text = $"Количество оборудования: {listEmployee.Count}";
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void ChoiseEquipment()
        {
            if (lvEquipment.SelectedItem is EF.Product selectEquipment)
            {
                if (selectEquipment.Qty < 1)
                {
                    MessageBox.Show("Оборудования нет в наличии", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    bool ret = false;
                    for (int i = 0; i < AppData.ListChoisenEquipment.Count(); i++)
                    {
                        if (AppData.ListChoisenEquipment[i].IdProduct == selectEquipment.IdProduct)
                        {
                            MessageBox.Show("Оборудование уже выбрано!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            ret = true;
                        }
                    }

                    if (ret == true)
                    {
                        ret = false;
                        return;
                    }

                    AppData.ListChoisenEquipment.Add(selectEquipment);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteEquipment()
        {
            if (AuthUser.Role.IdRole == 1)
            {
                if (lvEquipment.SelectedItem is EF.Product)
                {
                    try
                    {
                        var resClick = MessageBox.Show("Удалить выбранное Оборудование?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (resClick == MessageBoxResult.No)
                        {
                            return;
                        }


                        var selectedProduct = lvEquipment.SelectedItem as EF.Product;
                        selectedProduct.isDeleted = true;
                        AppData.Context.SaveChanges();
                        Filter();
                        MessageBox.Show("Оборудование успешно удалено!", "Информирование", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public void EditEquipment() 
        {
            if (lvEquipment.SelectedItem is EF.Product)
            {
                var selectedEquipment = lvEquipment.SelectedItem as EF.Product;
                if (selectedEquipment != null)
                {
                    AddEquipmentWindow addEquipmentWindow = new AddEquipmentWindow(selectedEquipment);
                    addEquipmentWindow.ShowDialog();
                    Filter();
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
                DeleteEquipment();
            }

            if (e.Key == Key.Back) 
            {
                EditEquipment();
            }

            if (e.Key == Key.Enter)
            {
                if (!EquipmentChoise)
                {
                    EditEquipment();
                }
                else
                {
                    ChoiseEquipment();
                }
            }
        }

        private void btnAddEquipment_Click(object sender, RoutedEventArgs e)
        {
            AddEquipmentWindow addEquipmentWindow = new AddEquipmentWindow();
            addEquipmentWindow.ShowDialog();
            Filter();
        }

        private void lvEquipment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (EquipmentChoise) 
            {
                ChoiseEquipment();
            }
            else
            {
                EditEquipment();
            }
        }
    }
}
