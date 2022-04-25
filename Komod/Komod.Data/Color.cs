using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class Color : BaseEntity
    {
        public string ColorCode { get; set; }
        public List<Article> Articles { get; set; }

        public Color()
        {
            Articles = new List<Article>();
        }
    }

    public class ColorMap
    {
        public ColorMap(EntityTypeBuilder<Color> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
        }
    }
}
