using Komod.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.CategoryModels
{
    public class CategoryViewModel
    {
        [HiddenInput]
        public long Id { get; set; }
        [Required(ErrorMessage = "Не указано название категории")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [HiddenInput]
        public long? ParentId { get; set; }
        [Display(Name = "Категория родителя")]
        public Category ParentCategory { get; set; }
        [Display(Name = "Категория родителя")]
        public SelectList ParentCategories { get; set; }
        [Display(Name = "Дата добавления")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Дата изменения")]
        public DateTime ModifiedDate { get; set; }
        public bool ChildCategories { get; set; }
        public bool MainPage {get;set;}
        [Display(Name = "Title")]
        public string Title {get;set;}
        [Display(Name = "Title Description")]
        public string TitleDescription{get;set;}

        //public bool ChildCategories { get; set; }
    }
}
