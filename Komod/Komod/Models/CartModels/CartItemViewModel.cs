using Komod.Web.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.CartModels
{
    public class CartItemViewModel
    {
        public ArticleViewModel Article { get; set; }
        public int ItemQuantity { get; set; }
        public bool MaxQuantity { get; set; }
        public decimal DiscountPercent { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public string PromocodeName{get;set;}
        public bool IsPromocodeExistForArticle {get;set;}
    }

    public class ProductFromSet
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public bool IsAlreadyAddToCart { get; set; }
    }
    public class CartProductSet
    {
        public List<ProductFromSet> ProductFromSets { get; set; }
        public decimal Discount { get; set; }
    }
}
