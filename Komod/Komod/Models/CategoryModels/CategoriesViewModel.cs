using Komod.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.CategoryModels
{
    public class CategoriesViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public int page { get; set; }
        public int sortType { get; set; }
        public string searchString { get; set; }
        public List<Category> CategoriesBred { get; set; }

        public List<CategoryViewModel> SubCategories { get; set; }

    }
}
