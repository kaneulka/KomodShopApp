using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.WishlistRepo
{
    public class WishlistItemRepository : IWishlistItemRepository
    {
        private readonly ApplicationContext context;
        private DbSet<WishlistItem> wishlistItems;
        string errorMessage = string.Empty;

        public WishlistItemRepository(ApplicationContext context)
        {
            this.context = context;
            wishlistItems = context.Set<WishlistItem>();
        }
        public IEnumerable<WishlistItem> GetAll()
        {
            return wishlistItems.AsEnumerable();
        }

        public WishlistItem Get(WishlistItem entity)
        {
            return wishlistItems.SingleOrDefault(s => s.WishlistId == entity.WishlistId && s.ProductId == entity.ProductId);
        }
        public void Insert(WishlistItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            wishlistItems.Add(entity);
            context.SaveChanges();
        }

        public void Update(WishlistItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(WishlistItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            wishlistItems.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(WishlistItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            wishlistItems.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
