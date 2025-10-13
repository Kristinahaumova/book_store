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

namespace Book_store
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow init;
        public UserContext CurrentUser { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Book_store.Classes.OrderItemContext> CartItems { get; set; } = new System.Collections.ObjectModel.ObservableCollection<Book_store.Classes.OrderItemContext>();
        public MainWindow()
        {
            InitializeComponent();
            init = this;
            OpenPage(new Pages.Authorization());
        }
        public void OpenPage(Page Page) 
        {
            frame.Navigate(Page);
        }
    }
}
