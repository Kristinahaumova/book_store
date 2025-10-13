using Book_store.Classes;
using Book_store.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class UserMainMenu : Page, INotifyPropertyChanged
    {
        private UserContext currentUser;
        private ObservableCollection<BookContext> _allBooks;
        private CollectionViewSource _filteredBooksView;
        private string _searchText = string.Empty;

        public ObservableCollection<BookContext> AllBooks
        {
            get => _allBooks;
            set
            {
                _allBooks = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public CollectionViewSource FilteredBooksView
        {
            get => _filteredBooksView;
            set
            {
                _filteredBooksView = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public UserMainMenu(UserContext user)
        {
            InitializeComponent();
            currentUser = user;
            DataContext = this;

            LoadBooks();
        }

        private void LoadBooks()
        {
            var allBooks = new BookContext().AllBooks();
            AllBooks = new ObservableCollection<BookContext>(allBooks);

            FilteredBooksView = new CollectionViewSource { Source = AllBooks };
            FilteredBooksView.Filter += Books_Filter;
            ApplyFilter();
        }

        private void Books_Filter(object sender, FilterEventArgs e)
        {
            if (e.Item is BookContext book)
            {
                bool searchMatch = string.IsNullOrEmpty(SearchText) ||
                                   book.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                   book.AuthorName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;

                e.Accepted = searchMatch;
            }
        }

        private void ApplyFilter()
        {
            if (FilteredBooksView?.View != null)
            {
                FilteredBooksView.View.Refresh();
            }
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
