using Komod.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.PropertyModels
{
    public class PropertiesViewModel
    {
        public IEnumerable<PropertyViewModel> Properties { get; set; }
        //public IEnumerable<Property> ProductProperties { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public int page { get; set; }
        public int sortType { get; set; }
        public string searchString { get; set; }
    }
}
