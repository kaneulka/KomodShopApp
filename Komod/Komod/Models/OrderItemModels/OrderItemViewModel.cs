using Komod.Data;
using Komod.Web.Models.ProductModels;
using Komod.Web.Models.PropertyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.OrderItemModels
{
    public class OrderItemViewModel
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public long OrderId { get; set; }
        public OrderViewModel Order { get; set; }
        public long ArticleId { get; set; }
        public ArticleViewModel Article { get; set; }
        public string ArticleName { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public List<PropertyValueViewModel> Properties {get;set;}
    }
}