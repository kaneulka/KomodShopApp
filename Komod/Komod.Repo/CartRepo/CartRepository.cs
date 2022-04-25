using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.CartRepo
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationContext context;
        private DbSet<Cart> carts;
        string errorMessage = string.Empty;

        public CartRepository(ApplicationContext context)
        {
            this.context = context;
            carts = context.Set<Cart>();
        }

        public Cart Get(long id)
        {
            return carts.Include(c=>c.CartItems).SingleOrDefault(s => s.Id == id);
        }
        public Cart GetByUser(string name)
        {
            return carts.Include(w => w.CartItems).SingleOrDefault(s => s.UserName == name);
        }
        public void Insert(Cart entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            carts.Add(entity);
            context.SaveChanges();
        }
        public void Delete(Cart entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            carts.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(Cart entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            carts.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
