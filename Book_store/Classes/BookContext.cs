using Book_store.Classes.Common;
using Book_store.Interfaces;
using Book_store.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Book_store.Classes
{
    public class BookContext : Book, IBook
    {
        public List<BookContext> AllBooks()
        {
            List<BookContext> allBooks = new List<BookContext>();
            MySqlConnection connection = Connection.OpenConnection();
            MySqlDataReader data = Connection.Query("SELECT * FROM `Books`", connection);
            while (data.Read())
            {
                BookContext book = new BookContext
                {
                    Id = data.GetInt32(0),
                    Name = data.GetString(1),
                    Description = data.GetString(2),
                    AuthorName = data.GetString(3),
                    BookRegion = data.GetString(4),
                    BookGenre = data.GetString(5),
                    Price = data.GetInt32(6)
                };
                allBooks.Add(book);
            }
            return allBooks;
        }

        public void Save(bool Update = false)
        {
            MySqlConnection connection = Connection.OpenConnection();
            if (Update)
            {
                Connection.Query("UPDATE `Books` " +
                                    "SET " +
                                        $"`Name` = '{this.Name}', " +
                                        $"`Description` = '{this.Description}', " +
                                        $"`AuthorName` = '{this.AuthorName}', " +
                                        $"`BookRegion` = '{this.BookRegion}', " +
                                        $"`BookGenre` = '{this.BookGenre}', " +
                                        $"`Price` = {this.Price} " +
                                    $"WHERE `Id` = {this.Id}", connection);
            }
            else
            {
                Connection.Query("INSERT INTO `Books` " +
                                    "(`Name`, `Description`, `AuthorName`, `BookRegion`, `BookGenre`, `Price`) " +
                                 $"VALUES ('{this.Name}', '{this.Description}', '{this.AuthorName}', '{this.BookRegion}', '{this.BookGenre}', {this.Price})", connection);
            }
        }

        public void Delete()
        {
            MySqlConnection connection = Connection.OpenConnection();
            Connection.Query($"DELETE FROM `Books` WHERE `Id` = {this.Id}", connection);
            MySqlConnection.ClearPool(connection);
        }
    }
}
