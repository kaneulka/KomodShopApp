using Komod.Web.Models.ProductModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.WishlistModels
{
    public class WishlistItemViewModel
    {
        [HiddenInput]
        public long WishlistId { get; set; }
        public WishlistViewModel Wishlist { get; set; }
        [HiddenInput]
        public long ProductId { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
