using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.PropertyModels
{
    public class PropertyValueViewModel
    {
        [Required]
        [HiddenInput]
        public long Id { get; set; }
        [Required(ErrorMessage = "Не указано значение")]
        [Display(Name = "Значение")]
        public string Value { get; set; }
        public string PropertyName {get;set;}

        [HiddenInput]
        public long PropertyId { get; set; }
    }
}
