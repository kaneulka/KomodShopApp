using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.CartRepo
{
    public interface ICartItemRepository
    {
        IEnumerable<CartItem> GetAll();
        CartItem Get(long id);
        void Insert(CartItem entity);
        void Update(CartItem entity);
        void Delete(CartItem entity);
        void Remove(CartItem entity);
        void SaveChanges();
    }
}
