using Komod.Data;
using Komod.Web.Models.BrandModels;
using Komod.Web.Models.CountryModels;
using Komod.Web.Models.ProductModels;
using Komod.Web.Models.PropertyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.CatalogModels
{
    public class FilterViewModel
    {
        public List<PropertyValueViewModel> PropertyValues { get; set; }
        public List<PropertyViewModel> Properties { get; set; }

        public List<BrandViewModel> Brands { get; set; }
        public List<CountryViewModel> Countries { get; set; }
        public List<ColorViewModel> Colors { get; set; }

        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
    }
}
