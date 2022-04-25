using Komod.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.AccountModels
{
    public class AccountViewModel
    {
        [HiddenInput]
        public string Id { get; set; }
        [Required(ErrorMessage = "Не указана почта")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "ФИО")]
        public string UserFIO { get; set; }
        [Display(Name = "Дата")]
        public string UserDate { get; set; }
        [Display(Name = "Месяц")]
        public string UserMonth { get; set; }
        [Display(Name = "Год")]
        public string UserYear { get; set; }
        [Display(Name = "Адресс")]
        public string Address { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }
        [Display(Name = "Контактный телефон")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

        public List<Order> Orders { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
