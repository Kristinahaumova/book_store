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
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Excel = Microsoft.Office.Interop.Excel;

namespace Book_store.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminMainMenu.xaml
    /// </summary>
    public partial class AdminMainMenu : Page
    {
        private UserContext currentUser;
        public AdminMainMenu(UserContext user = null)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void goToBooks(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new BooksPage(currentUser));
        }

        private void goToOrders(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new OrdersPage(currentUser));
        }

        private void goToUsers(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new UsersPage(currentUser));
        }

        private void CreateReport(object sender, RoutedEventArgs e)
        {
            var types = new[] { "Отчёт по книгам", "Отчёт по заказам", "Отчёт по пользователям" };
            var selectedType = MessageBox.Show("Выберите тип отчёта:\n1 - Книги\n2 - Заказы\n3 - Пользователи", "Создать отчёт", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            int typeIndex;
            if (selectedType == MessageBoxResult.Yes) typeIndex = 0;
            else if (selectedType == MessageBoxResult.No) typeIndex = 1;
            else if (selectedType == MessageBoxResult.Cancel) typeIndex = 2;
            else return;

            string reportType = types[typeIndex];
            string fileName = $"Отчёт_{reportType}_{DateTime.Now:yyyy-MM-dd_HH-mm}.xlsx";

            try
            {
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = false; 
                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                worksheet.Name = reportType;

                worksheet.Cells[1, 1] = reportType;
                ((Excel.Range)worksheet.Cells[1, 1]).Font.Bold = true;
                ((Excel.Range)worksheet.Cells[1, 1]).Font.Size = 14;

                int row = 2; 

                switch (typeIndex)
                {
                    case 0:
                        var books = new BookContext().AllBooks();
                        worksheet.Cells[row, 1] = "ID";
                        worksheet.Cells[row, 2] = "Название";
                        worksheet.Cells[row, 3] = "Автор";
                        worksheet.Cells[row, 4] = "Жанр";
                        worksheet.Cells[row, 5] = "Регион";
                        worksheet.Cells[row, 6] = "Цена";

                        for (int i = 0; i < books.Count; i++)
                        {
                            row++;
                            worksheet.Cells[row, 1] = books[i].Id;
                            worksheet.Cells[row, 2] = books[i].Name;
                            worksheet.Cells[row, 3] = books[i].AuthorName;
                            worksheet.Cells[row, 4] = books[i].BookGenre;
                            worksheet.Cells[row, 5] = books[i].BookRegion;
                            worksheet.Cells[row, 6] = books[i].Price;
                        }
                        break;
                    case 1:
                        var orders = new OrderContext().AllOrders();
                        worksheet.Cells[row, 1] = "ID";
                        worksheet.Cells[row, 2] = "Пользователь ID";
                        worksheet.Cells[row, 3] = "Время заказа";
                        worksheet.Cells[row, 4] = "Общая сумма";

                        for (int i = 0; i < orders.Count; i++)
                        {
                            row++;
                            worksheet.Cells[row, 1] = orders[i].Id;
                            worksheet.Cells[row, 2] = orders[i].IdUser;
                            worksheet.Cells[row, 3] = orders[i].OrderTime.ToString("dd.MM.yyyy HH:mm");
                            worksheet.Cells[row, 4] = orders[i].TotalCost;
                        }
                        break;
                    case 2:
                        var users = new UserContext().AllUsers();
                        worksheet.Cells[row, 1] = "ID";
                        worksheet.Cells[row, 2] = "Логин";
                        worksheet.Cells[row, 3] = "Роль (Админ)";

                        for (int i = 0; i < users.Count; i++)
                        {
                            row++;
                            worksheet.Cells[row, 1] = users[i].Id;
                            worksheet.Cells[row, 2] = users[i].Login;
                            worksheet.Cells[row, 3] = users[i].Role ? "Да" : "Нет";
                        }
                        break;
                }

                worksheet.Columns.AutoFit();

                var filePath = Path.Combine("D:\\Отчёты", fileName);
                workbook.SaveAs(filePath);
                workbook.Close();
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                MessageBox.Show($"Отчёт '{reportType}' сохранён: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчёта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
