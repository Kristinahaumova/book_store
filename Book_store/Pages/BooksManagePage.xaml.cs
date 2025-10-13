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
    /// Логика взаимодействия для BooksManagePage.xaml
    /// </summary>
    public partial class BooksManagePage : Page
    {
        private BookContext bookToManage;
        private UserContext currentAdminUser;
        private bool isEditMode;
        public BooksManagePage(BookContext book, UserContext adminUser, bool editMode = true)
        {
            InitializeComponent();
            bookToManage = book ?? new BookContext();
            currentAdminUser = adminUser;
            isEditMode = editMode;
            DataContext = bookToManage;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(bookToManage.Name))
            {
                MessageBox.Show("Название не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (bookToManage.Name.Length < 3)
            {
                MessageBox.Show("Название должно содержать минимум 3 символа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(bookToManage.AuthorName))
            {
                MessageBox.Show("Автор не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(bookToManage.BookRegion))
            {
                MessageBox.Show("Регион не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(bookToManage.BookGenre))
            {
                MessageBox.Show("Жанр не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (bookToManage.Price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(bookToManage.Description) && bookToManage.Description.Length < 10)
            {
                MessageBox.Show("Описание должно содержать минимум 10 символов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bookToManage.Save(isEditMode);
                MessageBox.Show(isEditMode ? "Книга обновлена!" : "Книга добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow.init.OpenPage(new BooksPage(currentAdminUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new BooksPage(currentAdminUser));
        }
    }
}
