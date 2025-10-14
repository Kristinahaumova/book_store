using Book_store.Classes;
using Book_store.Model;
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
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        private UserContext currentAdminUser;
        private OrderViewModel viewModel;
        public OrdersPage(UserContext adminUser)
        {
            InitializeComponent();
            currentAdminUser = adminUser;

            viewModel = new OrderViewModel();
            LoadOrders();
            DataContext = viewModel;
        }
        private void LoadOrders()
        {
            var allOrders = new OrderContext().AllOrders();
            viewModel.Orders = new ObservableCollection<OrderContext>(allOrders);
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new AdminMainMenu(currentAdminUser));
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is OrderContext orderToDelete)
            {
                var result = MessageBox.Show($"Удалить заказ #{orderToDelete.Id}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        orderToDelete.Delete();
                        viewModel.Orders.Remove(orderToDelete); 
                        MessageBox.Show("Заказ удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (button?.DataContext is OrderContext orderToEdit)
            {
                MainWindow.init.OpenPage(new OrdersManagePage(orderToEdit, currentAdminUser));
            }
        }

        private void AddOrder(object sender, RoutedEventArgs e)
        {
            var newOrder = new OrderContext { OrderItems = new System.Collections.Generic.List<OrderItem>() };
            MainWindow.init.OpenPage(new OrdersManagePage(newOrder, currentAdminUser, editMode: false));
        }
    }
}
