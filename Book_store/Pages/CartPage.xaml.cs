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
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        private UserContext currentUser;
        private decimal totalSum;
        public CartViewModel viewModel { get; private set; }

        public CartPage(UserContext user)
        {
            InitializeComponent();
            currentUser = user;

            LoadCartItems();
        }

        private void LoadCartItems()
        {
            var cartItems = MainWindow.init.CartItems.Select(item =>
            {
                var book = new BookContext().AllBooks().FirstOrDefault(b => b.Id == item.BookId) ?? new BookContext { Name = "Неизвестная книга", AuthorName = "Неизвестный автор" };
                return new CartItem { Item = item, Book = book };
            }).ToList();

            totalSum = cartItems.Sum(ci => (decimal)ci.Total);
            viewModel = new CartViewModel
            {
                CartItems = new ObservableCollection<CartItem>(cartItems),
                TotalCartSum = totalSum
            };

            DataContext = viewModel;
        }

        public void UpdateTotal()
        {
            totalSum = MainWindow.init.CartItems.Sum(item => (decimal)item.Quantity * item.Price);
            viewModel.TotalCartSum = totalSum;
        }

        private void Order(object sender, RoutedEventArgs e)
        {
            if (MainWindow.init.CartItems.Count == 0)
            {
                MessageBox.Show("Корзина пуста! Добавьте товары для оформления заказа.", "Корзина", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var orderItemsForOrder = MainWindow.init.CartItems.Select(item => (OrderItem)item).ToList();
                var order = new OrderContext
                {
                    IdUser = currentUser.Id,
                    OrderTime = DateTime.Now,
                    TotalCost = (int)totalSum,
                    OrderItems = orderItemsForOrder
                };
                order.Save(false);

                foreach (var item in MainWindow.init.CartItems.Cast<OrderItemContext>())
                {
                    item.OrderId = order.Id;
                    item.Save(false); 
                }

                order.Save(true);
                MessageBox.Show($"Заказ #{order.Id} успешно оформлен на сумму {totalSum} руб.!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow.init.CartItems.Clear();
                MainWindow.init.OpenPage(new UserMainMenu(currentUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при оформлении заказа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new UserMainMenu(currentUser));
        }
    }
}
