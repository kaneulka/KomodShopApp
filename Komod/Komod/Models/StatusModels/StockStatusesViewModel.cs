using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.StatusModels
{
    public class StockStatusesViewModel
    {
        public IEnumerable<StockStatusViewModel> StockStatuses { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
