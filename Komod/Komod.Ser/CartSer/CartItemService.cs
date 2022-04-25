using Komod.Data;
using Komod.Repo.CartRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.CartSer
{
    public class CartService : ICartService
    {
        private ICartRepository wishlistRepository;

        public CartService(ICartRepository wishlistRepository)
        {
            this.wishlistRepository = wishlistRepository;
        }

        public Cart GetCart(long id)
        {
            return wishlistRepository.Get(id);
        }
        public Cart GetCartByUser(string name)
        {
            return wishlistRepository.GetByUser(name);
        }

        public void InsertCart(Cart wishlist)
        {
            wishlistRepository.Insert(wishlist);
        }

        public void DeleteCart(long id)
        {
            Cart wishlist = GetCart(id);
            wishlistRepository.Remove(wishlist);
            wishlistRepository.SaveChanges();
        }
    }
}
