using Book_store.Classes;
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

namespace Book_store.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
            txtLogin.Focus();
        }

        private void Enter(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Логин не может быть пустым!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLogin.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(pwdPassword.Password))
            {
                MessageBox.Show("Пароль не может быть пустым!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Warning);
                pwdPassword.Focus();
                return;
            }

            try
            {
                var allUsers = new UserContext().AllUsers();
                var user = allUsers.FirstOrDefault(u => u.Login == txtLogin.Text && u.Password == pwdPassword.Password);

                if (user != null)
                {
                    MainWindow.init.CurrentUser = user;
                    if (user.Role)
                    {
                        MainWindow.init.OpenPage(new AdminMainMenu(user));
                    }
                    else
                    {
                        MainWindow.init.OpenPage(new UserMainMenu(user));
                    }

                    txtLogin.Clear();
                    pwdPassword.Clear();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    pwdPassword.Clear();
                    txtLogin.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Registration());
        }
    }
}
