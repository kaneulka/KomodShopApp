using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.WishlistSer
{
    public interface IWishlistService
    {
        Wishlist GetWishlist(long id);
        Wishlist GetWishlistByUser(string name);
        void InsertWishlist(Wishlist Wishlist);
        void DeleteWishlist(long id);
    }
}
