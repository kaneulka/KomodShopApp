using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.WishlistModels
{
    public class WishlistViewModel
    {
        [HiddenInput]
        public long Id { get; set; }
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
    }
}
