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
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var authUser = AppData.Context.Employee.ToList()
                .Where(i => i.Login.Equals(txtLogin.Text, StringComparison.CurrentCulture) 
                && i.Password.Equals(pswPassword.Password, StringComparison.CurrentCulture)).FirstOrDefault();

            if (authUser != null)
            {
                MainWindow mainWindow = new MainWindow(authUser);
                mainWindow.Show();
                this.Close();
            }
            else 
            {
                MessageBox.Show("Неправильный логин, или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text.Equals("Введите логин")) {
                txtLogin.Text = String.Empty;
            }
        }

        private void txtLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text.Equals(String.Empty)) {

                txtLogin.Text = "Введите логин";

            }
        }


    }
}
