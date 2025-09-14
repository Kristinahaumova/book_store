using Book_store.Classes.Common;
using Book_store.Interfaces;
using Book_store.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Book_store.Classes
{
    public class UserContext : User, IUser
    {
        public List<UserContext> AllUsers() 
        {
            List<UserContext> allUsers = new List<UserContext>();
            MySqlConnection connection = Connection.OpenConnection();
            MySqlDataReader data = Connection.Query("SELECT * FROM `Users`", connection);

            while (data.Read()) 
            {
                UserContext user = new UserContext
                {
                    Id = data.GetInt32(0),
                    Login = data.GetString(1),
                    Password = data.GetString(2)
                };
                allUsers.Add(user);
            }
            return allUsers;
        }

        public void Save(bool Update = false) 
        {
            MySqlConnection connection = Connection.OpenConnection();
            if (Update)
            {
                Connection.Query("UPDATE `Users` " +
                                    "SET " +
                                        $"`Login` = '{this.Login}', " +
                                        $"`Password` = '{this.Password}' " +
                                    $"WHERE `Id` = {this.Id}", connection);
            }
            else 
            {
                Connection.Query("INSERT INTO `Users` " + 
                                    "(`Login`, `Password`) " + 
                                 $"VALUES ('{this.Login}', '{this.Password}')", connection);
            }
        }

        public void Delete() 
        {
            MySqlConnection connection = Connection.OpenConnection();
            Connection.Query($"DELETE FROM `Users` WHERE `Id` = {this.Id}", connection);
            MySqlConnection.ClearPool(connection);
        }
    }
}
