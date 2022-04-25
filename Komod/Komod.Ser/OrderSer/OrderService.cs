using Komod.Data;
using Komod.Repo.OrderRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Ser.OrderSer
{
    public class OrderService : IOrderService
    {
        private IOrderRepository orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public List<Order> GetOrders()
        {
            return orderRepository.GetAll().ToList();
        }
        public Order GetOrder(long id)
        {
            return orderRepository.Get(id);
        }

        public void InsertOrder(Order order)
        {
            orderRepository.Insert(order);
        }

        public void DeleteOrder(long id)
        {
            Order order = GetOrder(id);
            orderRepository.Remove(order);
            orderRepository.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            orderRepository.Update(order);
        }
    }
}
