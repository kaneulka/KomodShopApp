using Komod.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.PromocodeModels
{
    public class PromocodeViewModel
    {
        [Required]
        [HiddenInput]
        public long Id { get; set; }
        [Required(ErrorMessage = "Не указано название фирмы")]
        [Display(Name = "Название")]
        public string PromocodeName { get; set; }
        [Display(Name = "Дата добавления")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Дата изменения")]
        public DateTime ModifiedDate { get; set; }
        [Display(Name = "Конец действия промокода")]
        public DateTime EndOfPromocode { get; set; }
        public decimal DiscountPercent { get; set; }
        public string PersonalUserPromo { get; set; }
        public int Count { get; set; }
    }

    public class PromocodeToDelete
    {
        public List<Promocode> Promocodes { get; set; }
        public Promocode Promocode { get; set; }
        public long Id { get; set; }
        public string EntityName { get; set; }
        public string URL { get; set; }
    }

    public class ReturnPromoJson
    {
        public PromocodeViewModel Promocode { get; set; }
        public List<string> ArticleNames { get; set; }
    }
}
