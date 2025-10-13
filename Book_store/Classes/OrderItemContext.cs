using Book_store.Classes.Common;
using Book_store.Interfaces;
using Book_store.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Book_store.Classes
{
    public class OrderItemContext : OrderItem, IOrderItem, INotifyPropertyChanged
    {
        private int _quantity;
        public new int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Total));
            }
        }
        public int Total => Quantity * Price;
        public List<OrderItemContext> AllOrderItems()
        {
            List<OrderItemContext> allItems = new List<OrderItemContext>();
            MySqlConnection connection = Connection.OpenConnection();
            MySqlDataReader data = Connection.Query("SELECT * FROM `OrderItems`", connection);
            while (data.Read())
            {
                OrderItemContext item = new OrderItemContext
                {
                    Id = data.GetInt32(0),
                    OrderId = data.GetInt32(1),
                    BookId = data.GetInt32(2),
                    Quantity = data.GetInt32(3),
                    Price = data.GetInt32(4)
                };
                allItems.Add(item);
            }
            return allItems;
        }

        public void Save(bool Update = false)
        {
            MySqlConnection connection = Connection.OpenConnection();
            try
            {
                if (Update)
                {
                    string sql = "UPDATE `OrderItems` " +
                                 "SET " +
                                     "`OrderId` = {0}, " +
                                     "`BookId` = {1}, " +
                                     "`Quantity` = {2}, " +
                                     "`Price` = {3} " +
                                 "WHERE `Id` = {4}";
                    Connection.ExecuteNonQuery(string.Format(sql, OrderId, BookId, Quantity, Price, Id), connection);
                }
                else
                {
                    string sql = "INSERT INTO `OrderItems` " +
                                 "(`OrderId`, `BookId`, `Quantity`, `Price`) " +
                                 "VALUES ({0}, {1}, {2}, {3})";
                    Connection.ExecuteNonQuery(string.Format(sql, OrderId, BookId, Quantity, Price), connection);
                }
            }
            finally
            {
                Connection.CloseConnection(connection);
            }
        }

        public void Delete()
        {
            MySqlConnection connection = Connection.OpenConnection();
            Connection.Query($"DELETE FROM `OrderItems` WHERE `Id` = {this.Id}", connection);
            MySqlConnection.ClearPool(connection);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
