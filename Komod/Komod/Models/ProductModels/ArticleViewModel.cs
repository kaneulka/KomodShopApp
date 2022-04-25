using Komod.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.ProductModels
{
    public class ArticleViewModel
    {
        [HiddenInput]
        public long Id { get; set; }
        [Required(ErrorMessage = "Не указано название артикула")]
        [RegularExpression(@"(\d*.)*", ErrorMessage = "Некорректный указан артикул")]
        [Display(Name = "Название артикула")]
        [Remote(action: "CheckArticleNames", controller: "Product", AdditionalFields = nameof(Id), ErrorMessage = "Этот артикул уже существует")]
        public string Name { get; set; }
        [Display(Name = "Дата добавления")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Дата изменения")]
        public DateTime ModifiedDate { get; set; }

        [Required(ErrorMessage = "Не указана цена артикула")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }
        [Display(Name = "Колличество товара")]
        public int Quantity { get; set; }
        [HiddenInput]
        public long StockStatusId { get; set; }
        public StockStatus StockStatus { get; set; }
        [HiddenInput]
        public long ProductId { get; set; }
        public Product Product { get; set; }
        [Display(Name = "Имя продукта")]
        public string ProductName { get; set; }
        public string ImagePath { get; set; }

        public SelectList StockStatuses { get; set; }
        public SelectList Properties { get; set; }
        public List<Property> AllProperties { get; set; }
        public SelectList PropertyValues { get; set; }
        public List<PropertyValue> PropertyValuesChecked { get; set; }
        public List<Property> PropertyChecked { get; set; }

        [HiddenInput]
        public long ColorId { get; set; }
        [Display(Name = "Название цвета")]
        public string ColorName { get; set; }
        [Display(Name = "Код цвета")]
        public string ColorCode { get; set; }
        public List<Color> Colors { get; set; }



        //[Required(ErrorMessage = "Не указано название артикула")]
        //[RegularExpression(@"(\d*.)*", ErrorMessage = "Некорректный указан артикул")]
        //[Display(Name = "Название артикула")]
        ////[Remote(action: "CheckArticleNamesEdit", controller: "Product", ErrorMessage = "Этот артикул уже существует")]
        //public string EditName { get; set; }
    }
}
