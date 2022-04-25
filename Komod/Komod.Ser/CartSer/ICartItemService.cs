using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.CartSer
{
    public interface ICartItemService
    {
        IEnumerable<CartItem> GetCartItems();
        CartItem GetCartItem(long id);
        void InsertCartItem(CartItem CartItem);
        void UpdateCartItem(CartItem CartItem);
        void DeleteCartItem(long id);
    }
}
