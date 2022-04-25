using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class Country : BaseEntity
    {
        public List<Product> Products { get; set; }

        public Country()
        {
            Products = new List<Product>();
        }
    }

    public class CountryMap
    {
        public CountryMap(EntityTypeBuilder<Country> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
        }
    }
}
