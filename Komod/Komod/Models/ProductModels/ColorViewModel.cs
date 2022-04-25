using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.ProductModels
{
    public class ColorViewModel
    {
        [Required]
        [HiddenInput]
        public long Id { get; set; }
        [Required(ErrorMessage = "Не указано название цвета")]
        [Display(Name = "Название цвета")]
        public string Name { get; set; }
        [Display(Name = "Код цвета")]
        public string ColorCode { get; set; }
        [Display(Name = "Дата добавления")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Дата изменения")]
        public DateTime ModifiedDate { get; set; }
        public long ArticleId { get; set; }
    }
}
