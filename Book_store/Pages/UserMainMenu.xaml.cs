using Book_store.Classes;
using Book_store.Elements;
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
    /// Логика взаимодействия для UserMainMenu.xaml
    /// </summary>
    public partial class UserMainMenu : Page
    {
        private UserContext currentUser;
        public UserMainMenu(UserContext user)
        {
            InitializeComponent();
            currentUser = user;

            var allBooks = new BookContext().AllBooks();
            DataContext = new { Books = new ObservableCollection<BookContext>(allBooks) };
        }


        private void Order(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new CartPage(currentUser));
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.init.CurrentUser = null;
            MainWindow.init.CartItems.Clear();
            MainWindow.init.OpenPage(new Authorization());
        }
    }
}
