using Komod.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.ProductModels
{
    public class ImageViewModel
    {
        [HiddenInput]
        public long Id { get; set; }
        [Display(Name = "Название изображения")]
        public string Name { get; set; }
        [Display(Name = "Дата добавления")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Дата изменения")]
        public DateTime ModifiedDate { get; set; }
        [Display(Name = "Путь изображения")]
        public string ImgPath { get; set; }
        [HiddenInput]
        public long ProductId { get; set; }
        public Product Product { get; set; }
        [Display(Name = "Главное ли изображение")]
        public bool MainImg { get; set; }
        public bool ArticleImage { get; set; }
    }
}
