using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class PropertyValue
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public long PropertyId { get; set; }
        public Property Property { get; set; }
        public List<PropertyValCatArt> PropertyValCatArts { get; set; }

        public PropertyValue()
        {
            PropertyValCatArts = new List<PropertyValCatArt>();
        }
    }
    public class PropertyValueMap
    {
        public PropertyValueMap(EntityTypeBuilder<PropertyValue> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Value).IsRequired();
        }
    }
}
