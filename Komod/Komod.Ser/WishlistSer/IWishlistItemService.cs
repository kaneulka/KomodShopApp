using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.WishlistSer
{
    public interface IWishlistItemService
    {
        IEnumerable<WishlistItem> GetWishlistItems();
        WishlistItem GetWishlistItem(WishlistItem wishlistItem);
        void InsertWishlistItem(WishlistItem wishlistItem);
        void UpdateWishlistItem(WishlistItem wishlistItem);
        void DeleteWishlistItem(WishlistItem wishlistItem);
    }
}
