using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class Wishlist
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public List<WishlistItem> WishlistItems { get; set; }

        public Wishlist()
        {
            WishlistItems = new List<WishlistItem>();
        }
    }
    public class WishlistMap
    {
        public WishlistMap(EntityTypeBuilder<Wishlist> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.UserName).IsRequired();
        }
    }
}
