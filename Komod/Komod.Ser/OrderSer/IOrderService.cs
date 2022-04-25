using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.OrderSer
{
    public interface IOrderService
    {
        public List<Order> GetOrders();
        Order GetOrder(long id);
        void InsertOrder(Order Order);
        void DeleteOrder(long id);
        void UpdateOrder(Order Order);
    }
}
