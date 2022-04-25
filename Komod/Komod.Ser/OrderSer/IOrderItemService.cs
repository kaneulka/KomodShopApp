using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.OrderSer
{
    public interface IOrderItemService
    {
        IEnumerable<OrderItem> GetOrderItems();
        OrderItem GetOrderItem(long id);
        void InsertOrderItem(OrderItem OrderItem);
        void InsertOrderItems(List<OrderItem> OrderItems);
        void UpdateOrderItem(OrderItem OrderItem);
        void DeleteOrderItem(long id);
    }
}
