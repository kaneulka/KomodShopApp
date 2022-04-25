using Komod.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.ProductModels
{
    public class ProductSetViewModel
    {
        [HiddenInput]
        public long Id { get; set; }
        [Display(Name = "Процент скидки")]
        public decimal DiscounPercent { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public List<Product> ProductsInAdmin { get; set; }
        [Display(Name = "Активный комплект?")]
        public bool ActiveSet { get; set; }
        [Display(Name = "Список продуктов в комплекте")]
        public string ProductSetName { get; set; }
        [Display(Name = "Название комплекта")]
        public string SetName { get; set; }
    }
}
