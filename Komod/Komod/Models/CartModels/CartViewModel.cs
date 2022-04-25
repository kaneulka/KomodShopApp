using Komod.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.CartModels
{
    public class CartViewModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public List<CartItem> CartItems { get; set; }
        //public string DiscountName {get;set;}
    }
}
