using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.EventPromotionModels
{
    public class EventPromotionsViewModel
    {
        public IEnumerable<EventPromotionViewModel> EventPromotions { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public int page { get; set; }
        public int sortType { get; set; }
        public string searchString { get; set; }
    }
}
