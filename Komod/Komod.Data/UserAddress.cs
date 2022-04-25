using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class UserAddress
    {
        public long Id { get; set; }
        public string FIO { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
    public class UserAddressMap
    {
        public UserAddressMap(EntityTypeBuilder<UserAddress> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.FIO).IsRequired();
            entityBuilder.Property(t => t.PhoneNumber);
            entityBuilder.Property(t => t.Email).IsRequired();
            entityBuilder.Property(t => t.Address).IsRequired();
            entityBuilder.Property(t => t.City).IsRequired();
            entityBuilder.Property(t => t.ZipCode);

            entityBuilder.HasOne(t => t.User).WithMany(t => t.UserAddresses).HasForeignKey(t=> t.UserId);
        }
    }
}
