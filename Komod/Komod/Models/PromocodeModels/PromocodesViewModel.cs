using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.PromocodeModels
{
    public class PromocodesViewModel
    {
        public IEnumerable<PromocodeViewModel> Promocodes { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
