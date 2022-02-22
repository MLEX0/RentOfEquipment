﻿using System;
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
    /// Логика взаимодействия для AddEditEmployeeWindow.xaml
    /// </summary>
    public partial class AddEditEmployeeWindow : Window
    {
        private bool isEdit = false;
        private EF.Employee editEmployee;
        private bool canQuitWithEsc = true;
        public AddEditEmployeeWindow()
        {
            InitializeComponent();

            cmbGender.ItemsSource = AppData.Context.Gender.ToList();
            cmbGender.DisplayMemberPath = "GenderName";
            cmbGender.SelectedIndex = 0;

            cmbRole.ItemsSource = AppData.Context.Role.ToList();
            cmbRole.DisplayMemberPath = "RoleName";
            cmbRole.SelectedIndex = 1;
        }

        public AddEditEmployeeWindow(EF.Employee changeEmployee) 
        {
            InitializeComponent();

            editEmployee = changeEmployee;
            isEdit = true;

            txtAddEditEmployee.Text = "Изменение сотрудника";

            cmbGender.ItemsSource = AppData.Context.Gender.ToList();
            cmbGender.DisplayMemberPath = "GenderName";

            cmbRole.ItemsSource = AppData.Context.Role.ToList();
            cmbRole.DisplayMemberPath = "RoleName";

            btnAddEmployee.Content = "Изменить";

            txtFName.Text = changeEmployee.FirstName;
            txtLName.Text = changeEmployee.LastName;
            txtMName.Text = changeEmployee.Patronymic;
            cmbGender.SelectedIndex = changeEmployee.IdGender - 1;
            txtEmail.Text = changeEmployee.Email;
            txtPhone.Text = changeEmployee.Phone;
            cmbRole.SelectedIndex = changeEmployee.IdRole - 1;
            txtLogin.Text = changeEmployee.Login;
            txtPassword.Text = changeEmployee.Password;

        }     
        
        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtFName.Text))
            {
                MessageBox.Show("Поле \"ИМЯ\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtFName.Text.Length > 60)
            {
                MessageBox.Show("Поле \"ИМЯ\" не может содержать более 60 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtLName.Text))
            {
                MessageBox.Show("Поле \"ФАМИЛИЯ\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtLName.Text.Length > 60)
            {
                MessageBox.Show("Поле \"ФАМИЛИЯ\" не может содержать более 60 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtMName.Text))
            {
                MessageBox.Show("Поле \"ОТЧЕСТВО\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtMName.Text.Length > 60)
            {
                MessageBox.Show("Поле \"ОТЧЕСТВО\" не может содержать более 60 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Поле \"ПОЧТА\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtEmail.Text.Length > 100)
            {
                MessageBox.Show("Поле \"ПОЧТА\" не может содержать более 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Поле \"Телефон\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtPhone.Text.Length > 12)
            {
                MessageBox.Show("Поле \"Телефон\" не может содержать более 12 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!ulong.TryParse(txtPhone.Text, out ulong res))
            {
                MessageBox.Show("Поле \"Телефон\" можно заполнить только цифрами", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Поле \"ЛОГИН\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtLogin.Text.Length > 100)
            {
                MessageBox.Show("Поле \"ЛОГИН\" не может содержать более 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Поле \"Пароль\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtPassword.Text.Length > 100)
            {
                MessageBox.Show("Поле \"Пароль\" не может содержать более 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!txtPassword.Text.Equals(txtRepeatPassword.Text))
            {
                MessageBox.Show("Поле \"Пароль\" и \"Повторите пароль\" не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            canQuitWithEsc = false;
            if (isEdit)
            {
                if (AppData.Context.Employee.Where(i => i.Login.Equals(txtLogin.Text) && i.Login != editEmployee.Login).FirstOrDefault() != null )
                {
                    MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {

                   editEmployee.FirstName = txtFName.Text;
                   editEmployee.LastName = txtLName.Text;
                   editEmployee.Patronymic = txtMName.Text;
                   editEmployee.IdGender = (cmbGender.SelectedItem as EF.Gender).IdGender;
                   editEmployee.Email = txtEmail.Text;
                   editEmployee.Phone = txtPhone.Text;
                   editEmployee.IdRole = (cmbRole.SelectedItem as EF.Role).IdRole;
                   editEmployee.Login = txtLogin.Text;
                   editEmployee.Password = txtPassword.Text;

                   AppData.Context.SaveChanges();

                   MessageBox.Show($"Пользователь {editEmployee.FIO} успешно изменён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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

                if (AppData.Context.Employee.Where(i => i.Login.Equals(txtLogin.Text)).FirstOrDefault() != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    EF.Employee addEmployee = new EF.Employee();

                    addEmployee.FirstName = txtFName.Text;
                    addEmployee.LastName = txtLName.Text;
                    addEmployee.Patronymic = txtMName.Text;
                    addEmployee.IdGender = (cmbGender.SelectedItem as EF.Gender).IdGender;
                    addEmployee.Email = txtEmail.Text;
                    addEmployee.Phone = txtPhone.Text;
                    addEmployee.IdRole = (cmbRole.SelectedItem as EF.Role).IdRole;
                    addEmployee.Login = txtLogin.Text;
                    addEmployee.Password = txtPassword.Text;

                    AppData.Context.Employee.Add(addEmployee);
                    AppData.Context.SaveChanges();

                    MessageBox.Show($"Пользователь {addEmployee.FIO} успешно создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && canQuitWithEsc) 
            {
                this.Close();
            }
        }

    }
}
