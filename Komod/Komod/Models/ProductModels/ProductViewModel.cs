using Komod.Data;
using Komod.Web.Models.PropertyModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.ProductModels
{
    public class ProductViewModel
    {
        [HiddenInput]
        public long Id { get; set; }

        [Required(ErrorMessage = "Не указано название продукта")]
        [RegularExpression(@"([a-zа-яё]|[A-ZА-ЯЁ]|[/(]|[/)]|\d|\s)*", ErrorMessage = "Некорректный указано название")]
        [Remote(action: "CheckProductName", controller: "Product", ErrorMessage = "Этот продукт уже существует")]
        [Display(Name = "Название продукта")]
        public string Name { get; set; }
        [Display(Name = "Дата добавления")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Дата изменения")]
        public DateTime ModifiedDate { get; set; }
        [HiddenInput]
        public long? BrandId { get; set; }
        [Display(Name = "Бренд")]
        public Brand Brand { get; set; }
        [HiddenInput]
        public long? CountryId { get; set; }
        [Display(Name = "Страна производитель")]
        public Country Country { get; set; }
        [HiddenInput]
        public long CategoryId { get; set; }
        [Display(Name = "Категория")]
        public Category Category { get; set; }
        [Display(Name = "Путь к папке")]
        public string Path { get; set; }
        [Display(Name = "Описание продукта")]
        public string Description { get; set; }
        [Display(Name = "Новинка")]
        public bool New { get; set; }
        [Display(Name = "Хит")]
        public bool Hit { get; set; }
        [Display(Name = "Минимальная цена")]
        public decimal MinProductPrice { get; set; }
        [Display(Name = "Максимальная цена")]
        public decimal MaxProductPrice { get; set; }
        [Display(Name = "Скидка (в проецнтах)")]
        public decimal DiscountPercent { get; set; }
        [Display(Name = "Дата работы скидки")]
        public DayOfWeek? DayOfWeek { get; set; }
        [Display(Name = "Товар в наличии")]
        public bool InStock { get; set; }

        [Display(Name = "title")]
        public string Title { get; set; }
        [Display(Name = "description")]
        public string TitleDescription { get; set; }

        public List<ImageViewModel> Images { get; set; }
        public List<ImageViewModel> ArticleImages { get; set; }
        public List<Article> Articles { get; set; }
        public List<ArticleViewModel> ArticlesVM { get; set; }
        public List<PropertyValCatArt> PropertyValCatArts { get; set; }
        public List<EventProduct> EventProducts { get; set; }

        public List<PropertyValueViewModel> PropertyValues { get; set; }
        public List<PropertyViewModel> Properties { get; set; }


        public List<ColorViewModel> Colors { get; set; }

        public SelectList Categories { get; set; }
        public SelectList Brands { get; set; }
        public SelectList Countries { get; set; }
        public string MainImgPath { get; set; }
        public List<Category> CategoriesBred { get; set; }
        //public List<PropertyWithArticle> ArticleProperties { get; set; }
    }

    public class PropertyWithArticle
    {
        public Property ArticleProperty { get; set; }
        public PropertyValue ArticlePropertyValue { get; set; }
        public long ArticleId { get; set; }
    }

    public class FullProduct
    {
        public ProductViewModel Product { get; set; }

        public List<ArticleViewModel> Articles { get; set; }

        public List<PropertyValueViewModel> PropertyValues { get; set; }
        public string MainImagePath { get; set; }
        public List<PropertyValCatArt> PVCA { get; set; }


        public List<PropertyViewModel> Properties { get; set; }
        public List<ColorViewModel> Colors { get; set; }
    }
}
