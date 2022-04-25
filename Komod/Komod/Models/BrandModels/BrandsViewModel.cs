using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.BrandModels
{
    public class BrandsViewModel
    {
        public IEnumerable<BrandViewModel> Brands { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
