using Komod.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.OrderItemModels
{
    public class OrderViewModel
    {
        [HiddenInput]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime AddedDate { get; set; }
        public string DeliveryMethodName { get; set; }
        [Display(Name = "Стоимость доставки")]
        public decimal DeliveryPrice { get; set; }
        public long PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public long OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }

        [Display(Name = "ФИО")]
        public string ClientFIO { get; set; }

        [Display(Name = "Телефон")]
        public string ClientPhone { get; set; }

        [Display(Name = "Доп. телефон")]
        public string ClientOtherPhone { get; set; }

        [Display(Name = "Email")]
        public string ClientEmail { get; set; }

        [Display(Name = "Адрес")]
        public string ClientAddress { get; set; }

        [Display(Name = "Комментарий")]
        public string Comment { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountPercent { get; set; }
        public string PromoName { get; set; }
    }
}