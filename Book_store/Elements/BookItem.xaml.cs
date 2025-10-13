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
using Book_store.Classes;
using Book_store.Model;
using Book_store.Pages;

namespace Book_store.Elements
{
    /// <summary>
    /// Логика взаимодействия для BookItem.xaml
    /// </summary>
    public partial class BookItem : UserControl
    {
        public BookItem()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is BookContext book)
            {
                MainWindow.init.OpenPage(new AboutBook(book));
            }
            e.Handled = true;
        }

        private void addCart(object sender, RoutedEventArgs e)
        {
            if (DataContext is BookContext book && MainWindow.init.CurrentUser != null)
            {
                var cartItem = new Classes.OrderItemContext
                {
                    OrderId = 0,
                    BookId = book.Id,
                    Quantity = 1,
                    Price = book.Price
                };

                var existingItem = MainWindow.init.CartItems?.FirstOrDefault(item => item.BookId == book.Id);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                    MessageBox.Show($"Количество '{book.Name}' увеличено до {existingItem.Quantity}!", "Корзина", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MainWindow.init.CartItems.Add(cartItem);
                    MessageBox.Show($"'{book.Name}' добавлена в корзину!", "Корзина", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Ошибка: Не удалось добавить в корзину. Проверьте авторизацию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
