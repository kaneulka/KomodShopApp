using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.CartSer
{
    public interface ICartService
    {
        Cart GetCart(long id);
        Cart GetCartByUser(string name);
        void InsertCart(Cart Cart);
        void DeleteCart(long id);
    }
}
