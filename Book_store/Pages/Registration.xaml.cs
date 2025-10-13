using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Book_store.Classes;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Book_store.Pages
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Логин не может быть пустым!", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLogin.Focus();
                return;
            }
            if (txtLogin.Text.Length < 3)
            {
                MessageBox.Show("Логин должен содержать минимум 3 символа!", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLogin.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(pwdPassword.Password))
            {
                MessageBox.Show("Пароль не может быть пустым!", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                pwdPassword.Focus();
                return;
            }
            if (pwdPassword.Password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов!", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                pwdPassword.Focus();
                return;
            }

            if (pwdPassword.Password != pwdConfirmPassword.Password)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                pwdConfirmPassword.Focus();
                return;
            }

            try
            {
                var allUsers = new UserContext().AllUsers();
                if (allUsers.Any(u => u.Login == txtLogin.Text))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtLogin.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке логина: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var newUser = new UserContext
                {
                    Login = txtLogin.Text,
                    Password = pwdPassword.Password,
                    Role = false 
                };
                newUser.Save(); 

                MessageBox.Show("Регистрация прошла успешно! Теперь вы можете авторизоваться.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Очистка полей
                txtLogin.Clear();
                pwdPassword.Clear();
                pwdConfirmPassword.Clear();

                MainWindow.init.OpenPage(new Authorization());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Authorization());
        }
    }
}
