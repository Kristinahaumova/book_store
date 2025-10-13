using Book_store.Classes.Common;
using Book_store.Interfaces;
using Book_store.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Book_store.Classes
{
    public class OrderContext : Order, IOrder
    {
        public List<OrderContext> AllOrders()
        {
            List<OrderContext> allOrders = new List<OrderContext>();
            using (MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;database=book_store;uid=root;"))  // Используй path из Connection
            {
                connection.Open();
                try
                {
                    using (MySqlDataReader data = new MySqlCommand("SELECT * FROM `Orders`", connection).ExecuteReader())
                    {
                        while (data.Read())
                        {
                            OrderContext order = new OrderContext
                            {
                                Id = data.GetInt32(0),
                                IdUser = data.GetInt32(1),
                                OrderTime = data.GetDateTime(2),
                                TotalCost = data.GetInt32(3)
                            };

                            // Загружаем items на отдельном connection, чтобы избежать nested reader
                            using (MySqlConnection itemConn = new MySqlConnection("server=localhost;port=3306;database=book_store;uid=root;"))
                            {
                                itemConn.Open();
                                using (MySqlDataReader itemData = new MySqlCommand($"SELECT * FROM `OrderItems` WHERE `OrderId` = {order.Id}", itemConn).ExecuteReader())
                                {
                                    while (itemData.Read())
                                    {
                                        order.OrderItems.Add(new OrderItemContext
                                        {
                                            Id = itemData.GetInt32(0),
                                            OrderId = itemData.GetInt32(1),
                                            BookId = itemData.GetInt32(2),
                                            Quantity = itemData.GetInt32(3),
                                            Price = itemData.GetInt32(4)
                                        });
                                    }
                                }
                            }

                            allOrders.Add(order);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Логируй ex, если нужно
                    throw;
                }
            }
            return allOrders;
        }

        public void Save(bool Update = false)
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;database=book_store;uid=root;"))
            {
                connection.Open();
                try
                {
                    if (Update)
                    {
                        string sql = "UPDATE `Orders` " +
                                     "SET " +
                                         "`IdUser` = @IdUser, " +
                                         "`OrderTime` = @OrderTime, " +
                                         "`TotalCost` = @TotalCost " +
                                     "WHERE `Id` = @Id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                        {
                            cmd.Parameters.AddWithValue("@IdUser", IdUser);
                            cmd.Parameters.AddWithValue("@OrderTime", OrderTime);
                            cmd.Parameters.AddWithValue("@TotalCost", TotalCost);
                            cmd.Parameters.AddWithValue("@Id", Id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string sql = "INSERT INTO `Orders` " +
                                     "(`IdUser`, `OrderTime`, `TotalCost`) " +
                                     "VALUES (@IdUser, @OrderTime, @TotalCost)";
                        using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                        {
                            cmd.Parameters.AddWithValue("@IdUser", IdUser);
                            cmd.Parameters.AddWithValue("@OrderTime", OrderTime);
                            cmd.Parameters.AddWithValue("@TotalCost", TotalCost);
                            cmd.ExecuteNonQuery();

                            // Получаем ID
                            cmd.CommandText = "SELECT LAST_INSERT_ID()";
                            Id = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }

                    if (Update)
                    {
                        using (MySqlCommand cmd = new MySqlCommand($"DELETE FROM `OrderItems` WHERE `OrderId` = @Id", connection))
                        {
                            cmd.Parameters.AddWithValue("@Id", Id);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    foreach (var item in OrderItems)
                    {
                        item.OrderId = Id;
                        ((OrderItemContext)item).Save(false);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Delete()
        {
            using (MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;database=book_store;uid=root;"))
            {
                connection.Open();
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand("DELETE FROM `OrderItems` WHERE `OrderId` = @Id", connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.ExecuteNonQuery();
                    }
                    using (MySqlCommand cmd = new MySqlCommand("DELETE FROM `Orders` WHERE `Id` = @Id", connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
