using Book_store.Classes.Common;
using Book_store.Interfaces;
using Book_store.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_store.Classes
{
    public class OrderItemContext : OrderItem, IOrderItem
    {
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
            if (Update)
            {
                Connection.Query("UPDATE `OrderItems` " +
                                    "SET " +
                                        $"`OrderId` = {this.OrderId}, " +
                                        $"`BookId` = {this.BookId}, " +
                                        $"`Quantity` = {this.Quantity}, " +
                                        $"`Price` = {this.Price} " +
                                    $"WHERE `Id` = {this.Id}", connection);
            }
            else
            {
                Connection.Query("INSERT INTO `OrderItems` " +
                                    "(`OrderId`, `BookId`, `Quantity`, `Price`) " +
                                 $"VALUES ({this.OrderId}, {this.BookId}, {this.Quantity}, {this.Price})", connection);
            }
        }

        public void Delete()
        {
            MySqlConnection connection = Connection.OpenConnection();
            Connection.Query($"DELETE FROM `OrderItems` WHERE `Id` = {this.Id}", connection);
            MySqlConnection.ClearPool(connection);
        }
    }
}
