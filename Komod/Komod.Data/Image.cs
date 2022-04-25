using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class Image:BaseEntity
    {
        public string ImgPath { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public bool MainImg { get; set; }
    }
    public class ImageMap
    {
        public ImageMap(EntityTypeBuilder<Image> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.Property(t => t.ImgPath).IsRequired();
            entityBuilder.Property(t => t.MainImg);

            entityBuilder.HasOne(o => o.Product).WithMany(c => c.Images).HasForeignKey(o => o.ProductId);
        }
    }
}
