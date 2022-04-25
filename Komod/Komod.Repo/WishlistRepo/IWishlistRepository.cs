using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.WishlistRepo
{
    public interface IWishlistRepository
    {
        Wishlist Get(long id);
        Wishlist GetByUser(string name);
        void Insert(Wishlist entity);
        void Delete(Wishlist entity);
        void Remove(Wishlist entity);
        void SaveChanges();
    }
}
