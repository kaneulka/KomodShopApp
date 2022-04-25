using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.ProductModels
{
    public class ProductSetsViewModel
    {
        public IEnumerable<ProductSetViewModel> ProductSets { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public int page { get; set; }
        public int sortType { get; set; }
        public string searchString { get; set; }
    }
}
