using Komod.Data;
using Komod.Repo.OrderStatusRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.OrderStatusSer
{
    public class OrderStatusService : IOrderStatusService
    {
        private IOrderStatusRepository orderStatusRepository;

        public OrderStatusService(IOrderStatusRepository orderStatusRepository)
        {
            this.orderStatusRepository = orderStatusRepository;
        }

        public IEnumerable<OrderStatus> GetOrderStatuses()
        {
            return orderStatusRepository.GetAll();
        }

        public OrderStatus GetOrderStatus(long id)
        {
            return orderStatusRepository.Get(id);
        }

        public void InsertOrderStatus(OrderStatus orderStatus)
        {
            orderStatusRepository.Insert(orderStatus);
        }
        public void UpdateOrderStatus(OrderStatus orderStatus)
        {
            orderStatusRepository.Update(orderStatus);
        }

        public void DeleteOrderStatus(long id)
        {
            OrderStatus orderStatus = GetOrderStatus(id);
            orderStatusRepository.Remove(orderStatus);
            orderStatusRepository.SaveChanges();
        }
    }
}
