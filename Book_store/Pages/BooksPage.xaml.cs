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
    /// Логика взаимодействия для BooksPage.xaml
    /// </summary>
    public partial class BooksPage : Page
    {
        private UserContext currentAdminUser;
        private BookViewModel viewModel;
        public BooksPage(UserContext adminUser)
        {
            InitializeComponent();
            currentAdminUser = adminUser;
            viewModel = new BookViewModel();
            LoadBooks();
            DataContext = viewModel;
        }
        private void LoadBooks()
        {
            var allBooks = new BookContext().AllBooks();
            viewModel.Books = new ObservableCollection<BookContext>(allBooks);
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new AdminMainMenu(currentAdminUser));
        }

        private void AddBook(object sender, RoutedEventArgs e)
        {
            var newBook = new BookContext();
            MainWindow.init.OpenPage(new BooksManagePage(newBook, currentAdminUser, editMode: false));
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is BookContext bookToEdit)
            {
                MainWindow.init.OpenPage(new BooksManagePage(bookToEdit, currentAdminUser));
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is BookContext bookToDelete)
            {
                var result = MessageBox.Show($"Удалить книгу '{bookToDelete.Name}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        bookToDelete.Delete();
                        viewModel.Books.Remove(bookToDelete); 
                        MessageBox.Show("Книга удалена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
