using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using RentOfEquipment.ClassHelper;

namespace RentOfEquipment.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEquipmentWindow.xaml
    /// </summary>
    public partial class AddEquipmentWindow : Window
    {
        private string pathImage = null;
        EF.Product EditEquipment { get; set; } = null;
        private bool canQuitWithEsc = true;
        public AddEquipmentWindow()
        {
            InitializeComponent();
            cmbCategory.ItemsSource = AppData.Context.Category.ToList();
            cmbCategory.DisplayMemberPath = "CategoryName";
            cmbCategory.SelectedIndex = 0;
        }

        public AddEquipmentWindow(EF.Product editEquipment)
        {
            this.EditEquipment = editEquipment;
            InitializeComponent();
            cmbCategory.ItemsSource = AppData.Context.Category.ToList();
            cmbCategory.DisplayMemberPath = "CategoryName";

            txtAddEditEquipment.Text = "Изменение оборудования";


            btnAddEquipment.Content = "Сохранить изменения";

            txtName.Text = EditEquipment.Name;
            txtDescription.Text = EditEquipment.Description;
            cmbCategory.SelectedItem = EditEquipment.Category;
            txtQty.Text = Convert.ToString(EditEquipment.Qty);
            txtCost.Text = Convert.ToString(EditEquipment.Cost);
            dpcWarranty.SelectedDate = EditEquipment.Warranty;

            if (EditEquipment.ProductImage != null)
            {
                using (MemoryStream stream = new MemoryStream(EditEquipment.ProductImage))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    imgEquipment.Source = bitmapImage;
                }
            }
            else
            {
                imgEquipment.Source = new BitmapImage(new Uri("/Res/nullImage.jpg", UriKind.Relative));
            }


        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && canQuitWithEsc)
            {
                this.Close();
            }
        }

        private void btnAddEquipment_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Поле \"НАИМЕНОВАНИЕ\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtName.Text.Length > 100)
            {
                MessageBox.Show("Поле \"НАИМЕНОВАНИЕ\" не может содержать более 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Поле \"ОПИСАНИЕ\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtDescription.Text.Length > 300)
            {
                MessageBox.Show("Поле \"ОПИСАНИЕ\" не может содержать более 300 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbCategory.SelectedItem == null || string.IsNullOrWhiteSpace(cmbCategory.Text))
            {
                MessageBox.Show("Поле \"КАТЕГОРИЯ\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtQty.Text))
            {
                MessageBox.Show("Поле \"КОЛИЧЕСТВО\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(txtQty.Text, out int res1))
            {
                MessageBox.Show("Поле \"КОЛИЧЕСТВО\" можно заполнить только цифрами", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtCost.Text))
            {
                MessageBox.Show("Поле \"СТОИМОСТЬ\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(txtCost.Text, out decimal res2))
            {
                MessageBox.Show("Поле \"СТОИМОСТЬ\" можно заполнить только цифрами", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (!dpcWarranty.SelectedDate.HasValue)
            {
                MessageBox.Show("Поле \"ГАРАНТИЯ\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            canQuitWithEsc = false;
            if (EditEquipment != null)
            {

                try
                {

                    // если нет фото передаём null
                    if (pathImage != null)
                    {
                        EditEquipment.ProductImage = File.ReadAllBytes(pathImage);
                    }

                    EditEquipment.Name = txtName.Text;
                    EditEquipment.Description = txtDescription.Text;
                    EditEquipment.IdCategory = (cmbCategory.SelectedItem as EF.Category).IdCategory;
                    EditEquipment.Qty = Convert.ToInt32(txtQty.Text);
                    EditEquipment.Cost = Convert.ToDecimal(txtCost.Text);
                    EditEquipment.Warranty = dpcWarranty.SelectedDate.Value;

                    AppData.Context.SaveChanges();

                    MessageBox.Show($"Оборудование {EditEquipment.Name} успешно изменено!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
                catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                canQuitWithEsc = true;
                return;
            }
            this.Close();
            }
            else
            {

                try
                {
                    EF.Product addProduct = new EF.Product();

                    // если нет фото передаём null
                    if (pathImage != null)
                    {
                        addProduct.ProductImage = File.ReadAllBytes(pathImage);
                    }
                    else
                    {
                        addProduct.ProductImage = null;
                    }

                    // если не выбрали фото, выводим вопрос с подтверждением
                    if (pathImage == null)
                    {
                        var resMess = MessageBox.Show("Фото не выбрано. Сохранить работника без фото?", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (resMess == MessageBoxResult.No)
                        {
                            return;
                        }
                    }

                    addProduct.Name = txtName.Text;
                    addProduct.Description = txtDescription.Text;
                    addProduct.IdCategory = (cmbCategory.SelectedItem as EF.Category).IdCategory;
                    addProduct.Qty = Convert.ToInt32(txtQty);
                    addProduct.Cost = Convert.ToDecimal(txtCost);
                    addProduct.Warranty = dpcWarranty.SelectedDate.Value;

                    AppData.Context.Product.Add(addProduct);
                    AppData.Context.SaveChanges();

                    MessageBox.Show($"Оборудование {addProduct.Name} успешно создано!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    canQuitWithEsc = true;
                    return;
                }
                this.Close();
            }
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            if (openFile.ShowDialog() == true)
            {
                imgEquipment.Source = new BitmapImage(new Uri(openFile.FileName));
                pathImage = openFile.FileName;
            }
        }

    }
}
