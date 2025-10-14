using Book_store.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        private UserContext currentAdminUser;
        private UserViewModel viewModel;
        public UsersPage(UserContext adminUser)
        {
            InitializeComponent();
            currentAdminUser = adminUser;
            viewModel = new UserViewModel();
            LoadUsers();
            DataContext = viewModel;
        }
        private void LoadUsers()
        {
            var allUsers = new UserContext().AllUsers();
            viewModel.Users = new ObservableCollection<UserContext>(allUsers);
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new AdminMainMenu(currentAdminUser));
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is UserContext userToDelete)
            {
                if (userToDelete.Id == currentAdminUser.Id)
                {
                    MessageBox.Show("Нельзя удалить самого себя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Удалить пользователя '{userToDelete.Login}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        userToDelete.Delete();
                        viewModel.Users.Remove(userToDelete); 
                        MessageBox.Show("Пользователь удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is UserContext userToEdit)
            {
                MainWindow.init.OpenPage(new UsersManagePage(userToEdit, currentAdminUser));
            }
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            var newUser = new UserContext { Role = false }; 
            MainWindow.init.OpenPage(new UsersManagePage(newUser, currentAdminUser, editMode: false));
        }
    }
}
