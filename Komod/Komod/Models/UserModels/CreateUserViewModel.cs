using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.UserModels
{
    public class CreateUserViewModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Логин")]
        public string UserName { get; set; }
        [Display(Name = "ФИО")]
        public string UserFIO { get; set; }
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
