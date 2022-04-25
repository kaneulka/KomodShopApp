using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class WishlistItem
    {
        public long WishlistId { get; set; }
        public Wishlist Wishlist { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
    public class WishlistItemMap
    {
        public WishlistItemMap(EntityTypeBuilder<WishlistItem> entityBuilder)
        {
            entityBuilder.HasKey(t => new { t.WishlistId, t.ProductId });

            entityBuilder.HasOne(t => t.Wishlist).WithMany(t => t.WishlistItems).HasForeignKey(t => t.WishlistId);
            entityBuilder.HasOne(t => t.Product).WithMany(t => t.WishlistItems).HasForeignKey(t => t.ProductId);
        }
    }
}
