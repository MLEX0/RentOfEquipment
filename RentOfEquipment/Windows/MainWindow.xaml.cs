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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RentOfEquipment.Windows;

namespace RentOfEquipment
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EF.Employee AuthUser { get; }
        public MainWindow(EF.Employee authUser)
        {
            InitializeComponent();
            AuthUser = authUser;
            txtAuthUser.Text = $"Вы вошли как: {AuthUser.IdEmployee} {AuthUser.FIO}";
        }

        private void btnMenuEmployee_Click(object sender, RoutedEventArgs e)
        {
            EmployeeListWindow employeeListWindow = new EmployeeListWindow(AuthUser);
            this.Opacity = 0.5;
            employeeListWindow.ShowDialog();
            this.Opacity = 1.0;
        }

        private void btnMenuClient_Click(object sender, RoutedEventArgs e)
        {
            ClientListWindow clientListWindow = new ClientListWindow(AuthUser);
            this.Opacity = 0.5;
            clientListWindow.ShowDialog();
            this.Opacity = 1.0;
        }

        private void btnMenuIssueOfEquipment_Click(object sender, RoutedEventArgs e)
        {
            IssuanceEquipmentWindow issuanceEquipmentWindow = new IssuanceEquipmentWindow(AuthUser);
            this.Opacity = 0.5;
            issuanceEquipmentWindow.ShowDialog();
            this.Opacity = 1.0;
        }

        private void btnEquipment_Click(object sender, RoutedEventArgs e)
        {
            EquipmentListWindow equipmentListWindow = new EquipmentListWindow(AuthUser);
            this.Opacity = 0.5;
            equipmentListWindow.ShowDialog();
            this.Opacity = 1.0;
        }

        private void btnProductEquipment_Click(object sender, RoutedEventArgs e)
        {
            IssuanceEquipmentListWindow issuanceEquipmentListWindow = new IssuanceEquipmentListWindow(AuthUser);
            this.Opacity = 0.5;
            issuanceEquipmentListWindow.ShowDialog();
            this.Opacity = 1.0;

        }
    }
}
