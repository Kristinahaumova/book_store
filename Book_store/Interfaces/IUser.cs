using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_store.Interfaces
{
    public interface IUser
    {
        void Save(bool Update = false);
        List<Classes.UserContext> AllUsers();
        void Delete();
    }
}
