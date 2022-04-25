using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.UserModels
{
    public class RoleViewModel
    {
        [HiddenInput]
        public string Id { get; set; }
        [Display(Name = "Название роли")]
        public string Name { get; set; }
    }
}
