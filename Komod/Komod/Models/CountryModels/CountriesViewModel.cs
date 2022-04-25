using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.CountryModels
{
    public class CountriesViewModel
    {
        public IEnumerable<CountryViewModel> Countries { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
