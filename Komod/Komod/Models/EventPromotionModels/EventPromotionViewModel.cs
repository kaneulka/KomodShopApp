using Komod.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.EventPromotionModels
{
    public class EventPromotionViewModel
    {
        [HiddenInput]
        public long Id { get; set; }
        [Required(ErrorMessage = "Не указано название акции")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Дата добавления")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Дата изменения")]
        public DateTime ModifiedDate { get; set; }
        [Display(Name = "Дата начала акции")]
        public DateTime StartEvent { get; set; }
        [Display(Name = "Дата окончания акции")]
        public DateTime EndEvent { get; set; }
        [Display(Name = "Описание акции")]
        public string Description { get; set; }
        public string ImgPath { get; set; }
        [Display(Name = "Активна ли акция")]
        public bool ActiveEvent { get; set; }
        public decimal DiscountPercent { get; set; }

        public List<EventProduct> EventProducts { get; set; }

        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
