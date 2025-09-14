using System.Collections.Generic;

namespace Book_store.Interfaces
{
    public interface IBook
    {
        void Save(bool Update = false);
        List<Classes.BookContext> AllBooks();
        void Delete();
    }
}
