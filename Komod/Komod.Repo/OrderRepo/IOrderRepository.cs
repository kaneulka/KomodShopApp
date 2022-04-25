using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.OrderRepo
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order Get(long id);
        void Insert(Order entity);
        void Update(Order entity);
        void Delete(Order entity);
        void Remove(Order entity);
        void SaveChanges();
    }
}
