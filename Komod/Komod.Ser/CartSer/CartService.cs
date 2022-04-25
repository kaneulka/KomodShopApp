using Komod.Data;
using Komod.Repo.CartRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.CartSer
{
    public class CartItemService : ICartItemService
    {
        private ICartItemRepository cartItemRepository;

        public CartItemService(ICartItemRepository cartItemRepository)
        {
            this.cartItemRepository = cartItemRepository;
        }

        public IEnumerable<CartItem> GetCartItems()
        {
            return cartItemRepository.GetAll();
        }

        public CartItem GetCartItem(long id)
        {
            return cartItemRepository.Get(id);
        }

        public void InsertCartItem(CartItem cartItem)
        {
            cartItemRepository.Insert(cartItem);
        }
        public void UpdateCartItem(CartItem cartItem)
        {
            cartItemRepository.Update(cartItem);
        }

        public void DeleteCartItem(long id)
        {
            CartItem cartItem = GetCartItem(id);
            cartItemRepository.Remove(cartItem);
            cartItemRepository.SaveChanges();
        }
    }
}
