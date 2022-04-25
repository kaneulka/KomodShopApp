using Komod.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.Methods
{
    public class DeliveryMethodViewModel
    {
        [HiddenInput]
        public long Id { get; set; }
        [Required(ErrorMessage = "Не указано название категории")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Район доставки")]
        public string District { get; set; }
        [Display(Name = "Стоимость доставки")]
        public int DeliveryPrice { get; set; }
        [Display(Name = "При какой стоимости заказа доставка бесплатная")]
        public int FreeDelivery { get; set; }
        //[HiddenInput]
        //public long? ParentId { get; set; }
        //[Display(Name = "Категория родителя")]
        //public Category ParentCategory { get; set; }
        //[Display(Name = "Категория родителя")]
        //public SelectList ParentCategories { get; set; }
        [Display(Name = "Дата добавления")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Дата изменения")]
        public DateTime ModifiedDate { get; set; }

        //public bool ChildCategories { get; set; }
    }
}
