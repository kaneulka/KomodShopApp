using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.WishlistRepo
{
    public interface IWishlistItemRepository
    {
        IEnumerable<WishlistItem> GetAll();
        WishlistItem Get(WishlistItem entity);
        void Insert(WishlistItem entity);
        void Update(WishlistItem entity);
        void Delete(WishlistItem entity);
        void Remove(WishlistItem entity);
        void SaveChanges();
    }
}
