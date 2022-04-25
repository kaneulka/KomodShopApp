using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.CartRepo
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationContext context;
        private DbSet<CartItem> cartItems;
        string errorMessage = string.Empty;

        public CartItemRepository(ApplicationContext context)
        {
            this.context = context;
            cartItems = context.Set<CartItem>();
        }
        public IEnumerable<CartItem> GetAll()
        {
            return cartItems.AsEnumerable();
        }

        public CartItem Get(long id)
        {
            return cartItems.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(CartItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            cartItems.Add(entity);
            context.SaveChanges();
        }

        public void Update(CartItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(CartItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            cartItems.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(CartItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            cartItems.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
