using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.PropertyModels
{
    public class PropertyViewModel
    {
        [HiddenInput]
        public long Id { get; set; }

        [Required(ErrorMessage = "Не указано название свойства")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Дата добавления")]
        public DateTime AddedDate { get; set; }

        [Display(Name = "Дата изменения")]
        public DateTime ModifiedDate { get; set; }

        //[Required(ErrorMessage = "Не указано значение")]
        [Display(Name = "Единица измерения")]
        public string ValueName { get; set; }

        public bool TurnOff { get; set; }

        //public List<Category> Categories { get; set; }
        //
        //public List<Category> AllCategories { get; set; }
    }
}