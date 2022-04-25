using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.CartRepo
{
    public interface ICartRepository
    {
        Cart Get(long id);
        Cart GetByUser(string name);
        void Insert(Cart entity);
        void Delete(Cart entity);
        void Remove(Cart entity);
        void SaveChanges();
    }
}
