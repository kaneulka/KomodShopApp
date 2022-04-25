using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.EventProductRepo
{
    public interface IEventProductRepository
    {
        IEnumerable<EventProduct> GetAll();
        EventProduct Get(EventProduct entity);
        void Insert(EventProduct entity);
        void InsertAll(List<EventProduct> entities);
        void Update(EventProduct entity);
        void Delete(EventProduct entity);
        void DeleteAll(List<EventProduct> entities);
        void Remove(EventProduct entity);
        void SaveChanges();
    }
}
