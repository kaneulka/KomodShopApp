using Komod.Data;
using System;
using System.Collections.Generic;

namespace Komod.Repo.OrderRepo
{
    public interface IOrderItemRepository
    {
        IEnumerable<OrderItem> GetAll();
        OrderItem Get(long id);
        void Insert(OrderItem entity);
        void Update(OrderItem entity);
        void Delete(OrderItem entity);
        void Remove(OrderItem entity);
        void SaveChanges();
    }
}
