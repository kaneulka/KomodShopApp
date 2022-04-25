using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class User : IdentityUser
    {
        public string UserFIO { get; set; }

        public List<UserAddress> UserAddresses { get; set; }
        public User()
        {
            UserAddresses = new List<UserAddress>();
        }
    }
}
