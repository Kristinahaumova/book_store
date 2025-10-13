using Book_store.Classes;
using Book_store.Model;
using Book_store.Pages;
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

namespace Book_store.Elements
{
    /// <summary>
    /// Логика взаимодействия для OrderItem.xaml
    /// </summary>
    public partial class OrderItem : UserControl
    {
        public OrderItem()
        {
            InitializeComponent();
        }

        private void Plus(object sender, RoutedEventArgs e)
        {
            if (DataContext is CartItem cartItem)
            {
                cartItem.Item.Quantity++;
                txtCount.Text = cartItem.Item.Quantity.ToString();
                UpdateParentTotal();
            }
        }

        private void Minus(object sender, RoutedEventArgs e)
        {
            if (DataContext is CartItem cartItem && cartItem.Item.Quantity > 1)
            {
                cartItem.Item.Quantity--;
                txtCount.Text = cartItem.Item.Quantity.ToString();
                UpdateParentTotal();
            }
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is CartItem cartItem)
            {
                var result = MessageBox.Show("Удалить эту позицию?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    MainWindow.init.CartItems.Remove(cartItem.Item);
                    var parentPage = FindParent<CartPage>(this);
                    if (parentPage?.viewModel != null)
                    {
                        parentPage.viewModel.CartItems.Remove(cartItem);
                    }
                    UpdateParentTotal();
                }
            }
        }

        private void Quantity_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is CartItem cartItem)
            {
                if (int.TryParse(((TextBox)sender).Text, out int newQuantity) && newQuantity > 0)
                {
                    cartItem.Item.Quantity = newQuantity;
                    UpdateParentTotal();
                }
                else
                {
                    ((TextBox)sender).Text = cartItem.Item.Quantity.ToString();
                }
            }
        }

        private void UpdateParentTotal()
        {
            var parentPage = FindParent<CartPage>(this);
            parentPage?.UpdateTotal();
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            if (parentObject is T parent)
                return parent;
            return FindParent<T>(parentObject);
        }
    }
}
