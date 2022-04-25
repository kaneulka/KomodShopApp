using Komod.Data;
using Komod.Repo.OrderRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.OrderSer
{
    public class OrderItemService : IOrderItemService
    {
        private IOrderItemRepository orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            this.orderItemRepository = orderItemRepository;
        }

        public IEnumerable<OrderItem> GetOrderItems()
        {
            return orderItemRepository.GetAll();
        }

        public OrderItem GetOrderItem(long id)
        {
            return orderItemRepository.Get(id);
        }

        public void InsertOrderItem(OrderItem orderItem)
        {
            orderItemRepository.Insert(orderItem);
        }
        public void InsertOrderItems(List<OrderItem> orderItems)
        {
            foreach(var orderItem in orderItems)
            {
                orderItemRepository.Insert(orderItem);
            }
        }
        public void UpdateOrderItem(OrderItem orderItem)
        {
            orderItemRepository.Update(orderItem);
        }

        public void DeleteOrderItem(long id)
        {
            OrderItem orderItem = GetOrderItem(id);
            orderItemRepository.Remove(orderItem);
            orderItemRepository.SaveChanges();
        }
    }
}
