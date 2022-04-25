using Komod.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.OrderItemModels
{
    public class OrderInfo
    {
        public OrderViewModel Order { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
    }
}