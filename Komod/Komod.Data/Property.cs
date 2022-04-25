using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class Property :BaseEntity
    {
        public string ValueName { get; set; }
        public bool TurnOff { get; set; }


        public List<PropertyValue> PropertyValues { get; set; }

        public Property()
        {
            PropertyValues = new List<PropertyValue>();
        }
    }
    public class PropertyMap
    {
        public PropertyMap(EntityTypeBuilder<Property> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.Property(t => t.ValueName).IsRequired();

            entityBuilder.HasMany(t => t.PropertyValues).WithOne(t => t.Property).HasForeignKey(t=>t.PropertyId);
        }
    }
}
