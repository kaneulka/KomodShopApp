using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.OrderStatusSer
{
    public interface IOrderStatusService
    {
        IEnumerable<OrderStatus> GetOrderStatuses();
        OrderStatus GetOrderStatus(long id);
        void InsertOrderStatus(OrderStatus order);
        void UpdateOrderStatus(OrderStatus order);
        void DeleteOrderStatus(long id);
    }
}
