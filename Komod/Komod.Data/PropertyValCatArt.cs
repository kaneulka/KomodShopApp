using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class PropertyValCatArt
    {
        public long ArticleId { get; set; }
        public Article Article { get; set; }
        public long PropertyValueId { get; set; }
        public PropertyValue PropertyValue { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }

    }
    public class PropertyValCatArtMap
    {
        public PropertyValCatArtMap(EntityTypeBuilder<PropertyValCatArt> entityBuilder)
        {
            entityBuilder.HasKey(t => new { t.ArticleId, t.CategoryId, t.PropertyValueId, t.ProductId });
            entityBuilder.Property(t => t.ArticleId).IsRequired();
            entityBuilder.Property(t => t.PropertyValueId).IsRequired();
            entityBuilder.Property(t => t.CategoryId).IsRequired();
            entityBuilder.Property(t => t.ProductId).IsRequired();

            entityBuilder.HasOne(t => t.PropertyValue).WithMany(t => t.PropertyValCatArts).HasForeignKey(t=> t.PropertyValueId).OnDelete(DeleteBehavior.ClientCascade);
            entityBuilder.HasOne(t => t.Article).WithMany(t => t.PropertyValCatArts).HasForeignKey(t => t.ArticleId).OnDelete(DeleteBehavior.ClientCascade);
            entityBuilder.HasOne(t => t.Category).WithMany(t => t.PropertyValCatArts).HasForeignKey(t => t.CategoryId).OnDelete(DeleteBehavior.ClientCascade);
            entityBuilder.HasOne(t => t.Product).WithMany(t => t.PropertyValCatArts).HasForeignKey(t => t.ProductId).OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
