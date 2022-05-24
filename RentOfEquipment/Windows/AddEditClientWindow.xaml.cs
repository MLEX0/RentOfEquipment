using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddEditClientWindow.xaml
    /// </summary>
    public partial class AddEditClientWindow : Window
    {
        private EF.Client EditClient { get; set; } = null;

        public AddEditClientWindow()
        {
            InitializeComponent();
            cmbGender.ItemsSource = AppData.Context.Gender.ToList();
            cmbGender.DisplayMemberPath = "GenderName";
            cmbGender.SelectedIndex = 0;
        }

        public AddEditClientWindow(EF.Client client) 
        {
            InitializeComponent();
            cmbGender.ItemsSource = AppData.Context.Gender.ToList();
            cmbGender.DisplayMemberPath = "GenderName";

            txtAddEditClient.Text = "Изменение клиента";

            btnAddClient.Content = "Сохранить изменения";

            EditClient = client;
            txtFName.Text = EditClient.FirstName;
            txtLName.Text = EditClient.LastName;
            txtMName.Text = EditClient.Patronymic;
            cmbGender.SelectedItem = EditClient.Gender;
            dbcBirthday.SelectedDate = EditClient.Birthday;
            txtEmail.Text = EditClient.Email;
            txtPhone.Text = EditClient.Phone;
            txtPasSerial.Text = EditClient.Passport.Serial;
            txtPasNumber.Text = EditClient.Passport.Number;


        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            bool IsValidEmail(string Email) 
            {
                string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
                Match isMath = Regex.Match(Email, pattern, RegexOptions.IgnoreCase);
                return isMath.Success;
            }

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


            if (dbcBirthday.SelectedDate.Value == null)
            {
                MessageBox.Show("Поле \"Дата рождения\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dbcBirthday.SelectedDate.Value > DateTime.Now)
            {
                MessageBox.Show("Значение поля \"Дата рождения\" не может быть больше текущей даты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Поле \"ПОЧТА\" не соответствует требованиям", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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


            if (String.IsNullOrWhiteSpace(txtPasSerial.Text))
            {
                MessageBox.Show("Поле \"Серия паспорта\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtPasSerial.Text.Length > 10)
            {
                MessageBox.Show("Поле \"Серия паспорта\" не может содержать более 10 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!ulong.TryParse(txtPasSerial.Text, out ulong res2))
            {
                MessageBox.Show("Поле \"Серия паспорта\" можно заполнить только цифрами", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (String.IsNullOrWhiteSpace(txtPasNumber.Text))
            {
                MessageBox.Show("Поле \"Номер паспорта\" не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtPasNumber.Text.Length > 10)
            {
                MessageBox.Show("Поле \"Номер паспорта\" не может содержать более 10 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!ulong.TryParse(txtPasNumber.Text, out ulong res3))
            {
                MessageBox.Show("Поле \"Номер паспорта\" можно заполнить только цифрами", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var checkPass = AppData.Context.Passport.Where(i => i.Serial == txtPasSerial.Text 
                                                            && i.Number == txtPasNumber.Text 
                                                            && i.Number != EditClient.Passport.Number 
                                                            && i.Serial != EditClient.Passport.Serial).ToList().FirstOrDefault();
            if (checkPass != null) 
            {
                MessageBox.Show("Данный паспорт уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (EditClient != null)
            {

                try
                {

                    EditClient.FirstName = txtFName.Text;
                    EditClient.LastName = txtLName.Text;
                    EditClient.Patronymic = txtMName.Text;
                    EditClient.IdGender = (cmbGender.SelectedItem as EF.Gender).IdGender;
                    EditClient.Email = txtEmail.Text;
                    EditClient.Phone = txtPhone.Text;
                    EditClient.Birthday = dbcBirthday.SelectedDate.Value;
                    EditClient.Passport.Number = txtPasNumber.Text;
                    EditClient.Passport.Serial = txtPasSerial.Text;

                    AppData.Context.SaveChanges();

                    MessageBox.Show($"Клиент {EditClient.FIO} успешно изменён!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                this.Close();
            }
            else
            {

                try
                {
                    EF.Passport addPass = new EF.Passport();
                    addPass.Serial = txtPasSerial.Text;
                    addPass.Number = txtPasNumber.Text;
                    AppData.Context.Passport.Add(addPass);
                    AppData.Context.SaveChanges();

                    EF.Passport AddedPass = AppData.Context.Passport.Where(i => i.Number == addPass.Number && i.Serial == addPass.Serial).ToList().FirstOrDefault();


                    EF.Client addClient = new EF.Client();
                    addClient.FirstName = txtFName.Text;
                    addClient.LastName = txtLName.Text;
                    addClient.Patronymic = txtMName.Text;
                    addClient.IdGender = (cmbGender.SelectedItem as EF.Gender).IdGender;
                    addClient.Email = txtEmail.Text;
                    addClient.Phone = txtPhone.Text;
                    addClient.Birthday = dbcBirthday.SelectedDate.Value;
                    addClient.IdPasssport = AddedPass.IdPassport;

                    AppData.Context.Client.Add(addClient);
                    AppData.Context.SaveChanges();

                    MessageBox.Show($"Пользователь {addClient.FIO} успешно создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                this.Close();
            }
        }

        private void txtPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "1234567890+".IndexOf(e.Text) < 0;
        }
    }
}
