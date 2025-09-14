using System.Collections.Generic;

namespace Book_store.Interfaces
{
    public interface IOrder
    {
        void Save(bool Update = false);
        List<Classes.OrderContext> AllOrders();
        void Delete();
    }
}
