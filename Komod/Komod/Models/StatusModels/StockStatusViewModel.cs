using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.StatusModels
{
    public class StockStatusViewModel
    {
        [Required]
        [HiddenInput]
        public long Id { get; set; }
        [Required(ErrorMessage = "Не указано название статуса товара")]
        [Display(Name = "Название статуса товара")]
        public string Name { get; set; }
        [Display(Name = "Дата добавления")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Дата изменения")]
        public DateTime ModifiedDate { get; set; }
    }
}
