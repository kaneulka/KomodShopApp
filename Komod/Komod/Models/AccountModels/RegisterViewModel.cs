using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.AccountModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Не указана почта")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Эта почта уже используется")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан логин")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Логин должен быть не короче 3 символов")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Не указан день рождения")]
        //[Range(1, 31, ErrorMessage = "Недопустимый число")]
        //[Display(Name = "Дата")]
        //public string Date { get; set; }

        //[Required(ErrorMessage = "Не указан месяц рождения")]
        //[Range(1, 12, ErrorMessage = "Недопустимый месяц")]
        //[Display(Name = "Месяц")]
        //public string Month { get; set; }

        //[Required(ErrorMessage = "Не указан год рождения")]
        //[Range(1920, 2018, ErrorMessage = "Недопустимый год")]
        //[Display(Name = "Год")]
        //public string Year { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [Remote(action: "CheckPassword", controller: "Account", ErrorMessage = "Пароль должен соcтоять из заглавных и строчных латинских символов, цифр и содержать хотя бы один знак. Длина пароля должна быть не менее 6.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
