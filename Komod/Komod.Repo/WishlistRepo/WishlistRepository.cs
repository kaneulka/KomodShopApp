using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.WishlistRepo
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly ApplicationContext context;
        private DbSet<Wishlist> wishlists;
        string errorMessage = string.Empty;

        public WishlistRepository(ApplicationContext context)
        {
            this.context = context;
            wishlists = context.Set<Wishlist>();
        }

        public Wishlist Get(long id)
        {
            return wishlists.Include(w=>w.WishlistItems).SingleOrDefault(s => s.Id == id);
        }
        public Wishlist GetByUser(string name)
        {
            return wishlists.Include(w => w.WishlistItems).SingleOrDefault(s => s.UserName == name);
        }
        public void Insert(Wishlist entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            wishlists.Add(entity);
            context.SaveChanges();
        }
        public void Delete(Wishlist entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            wishlists.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(Wishlist entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            wishlists.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
