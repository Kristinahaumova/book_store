using Book_store.Classes;
using Book_store.Model;
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
    /// Логика взаимодействия для OrdersManagePage.xaml
    /// </summary>
    public partial class OrdersManagePage : Page
    {
        private OrderContext orderToManage;
        private UserContext currentAdminUser;
        private bool isEditMode;

        public OrdersManagePage(OrderContext order, UserContext adminUser, bool editMode = true)
        {
            InitializeComponent();
            orderToManage = order ?? new OrderContext { OrderItems = new System.Collections.Generic.List<OrderItem>() };
            currentAdminUser = adminUser;
            isEditMode = editMode;
            DataContext = orderToManage;

            if (!isEditMode)
            {
                orderToManage.OrderTime = DateTime.Now;
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new OrdersPage(currentAdminUser));
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (orderToManage.OrderTime == DateTime.MinValue)
            {
                MessageBox.Show("Укажите время заказа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (orderToManage.OrderItems.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы одну позицию в заказ!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            orderToManage.TotalCost = (int)orderToManage.OrderItems.Sum(item => item.Quantity * item.Price);

            try
            {
                orderToManage.Save(isEditMode);
                foreach (var item in orderToManage.OrderItems)
                {
                    ((OrderItemContext)item).Save(isEditMode); 
                }

                MessageBox.Show(isEditMode ? "Заказ обновлен!" : "Заказ создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow.init.OpenPage(new OrdersPage(currentAdminUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is OrderItemContext itemToDelete)
            {
                var result = MessageBox.Show("Удалить эту позицию?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    orderToManage.OrderItems.Remove(itemToDelete);
                    MessageBox.Show("Позиция удалена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
