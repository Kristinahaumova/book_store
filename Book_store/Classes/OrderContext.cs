using Book_store.Classes.Common;
using Book_store.Interfaces;
using Book_store.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Book_store.Classes
{
    public class OrderContext : Order, IOrder
    {
        public List<OrderContext> AllOrders()
        {
            List<OrderContext> allOrders = new List<OrderContext>();
            MySqlConnection connection = Connection.OpenConnection();
            MySqlDataReader data = Connection.Query("SELECT * FROM `Orders`", connection);
            while (data.Read())
            {
                OrderContext order = new OrderContext
                {
                    Id = data.GetInt32(0),
                    IdUser = data.GetInt32(1),
                    OrderTime = data.GetDateTime(2),
                    TotalCost = data.GetInt32(3)
                };
                var itemData = Connection.Query("SELECT * FROM `OrderItems`" +
                                                $" WHERE `OrderId` = {order.Id}", connection);
                while (itemData.Read()) 
                {
                    order.OrderItems.Add(new OrderItem 
                    {
                        Id = itemData.GetInt32(0),
                        OrderId = itemData.GetInt32(1),
                        BookId = itemData.GetInt32(2),
                        Quantity = itemData.GetInt32(3),
                        Price = itemData.GetInt32(4)
                    });
                }
                allOrders.Add(order);
            }
            return allOrders;
        }

        public void Save(bool Update = false)
        {
            MySqlConnection connection = Connection.OpenConnection();
            if (Update)
            {
                Connection.Query("UPDATE `Orders` " +
                                    "SET " +
                                        $"`IdUser` = {this.IdUser}, " +
                                        $"`OrderTime` = '{this.OrderTime.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                                        $"`TotalCost` = {this.TotalCost} " +
                                    $"WHERE `Id` = {this.Id}", connection);
            }
            else
            {
                Connection.Query("INSERT INTO `Orders` " +
                                    "(`IdUser`, `OrderTime`, `TotalCost`) " +
                                 $"VALUES ({this.IdUser}, '{this.OrderTime.ToString("yyyy-MM-dd HH:mm:ss")}', {this.TotalCost})", connection);
            }
        }

        public void Delete()
        {
            MySqlConnection connection = Connection.OpenConnection();
            Connection.Query($"DELETE FROM `Orders` WHERE `Id` = {this.Id}", connection);
            MySqlConnection.ClearPool(connection);
        }
    }
}
