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
    /// Логика взаимодействия для UsersManagePage.xaml
    /// </summary>
    public partial class UsersManagePage : Page
    {
        private UserContext userToManage;
        private UserContext currentAdminUser;
        private bool isEditMode;
        public UsersManagePage(UserContext user, UserContext adminUser, bool editMode = true)
        {
            InitializeComponent();
            userToManage = user ?? new UserContext { Role = false };
            currentAdminUser = adminUser;
            isEditMode = editMode;
            DataContext = userToManage;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new UsersPage(currentAdminUser));
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userToManage.Login))
            {
                MessageBox.Show("Логин не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (userToManage.Login.Length < 3)
            {
                MessageBox.Show("Логин должен содержать минимум 3 символа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string password = passwordPwdBox.Password;
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пароль не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var allUsers = new UserContext().AllUsers();
            var existingUser = allUsers.FirstOrDefault(u => u.Login == userToManage.Login && u.Id != userToManage.Id);
            if (existingUser != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            userToManage.Password = password;

            try
            {
                userToManage.Save(isEditMode);

                MessageBox.Show(isEditMode ? "Пользователь обновлен!" : "Пользователь добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                passwordPwdBox.Clear();
                MainWindow.init.OpenPage(new UsersPage(currentAdminUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
