using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class Brand : BaseEntity
    {
        public List<Product> Products { get; set; }

        public Brand()
        {
            Products = new List<Product>();
        }
    }

    public class BrandMap
    {
        public BrandMap(EntityTypeBuilder<Brand> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
        }
    }
}
