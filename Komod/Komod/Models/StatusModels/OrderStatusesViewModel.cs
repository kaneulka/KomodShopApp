using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.StatusModels
{
    public class OrderStatusesViewModel
    {
        public IEnumerable<OrderStatusViewModel> OrderStatuses { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
