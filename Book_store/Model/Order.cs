using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_store.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public DateTime OrderTime { get; set; }
        public int TotalCost { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Order(int idUser, DateTime orderTime, int totalCost) 
        {
            IdUser = idUser;
            OrderTime = orderTime;
            TotalCost = totalCost;
        }
    }
}
