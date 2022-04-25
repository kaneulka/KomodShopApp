using Komod.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.PromocodeModels
{
    public class PromocodeArticleModel
    {
        [HiddenInput]
        public long PromocodeId { get; set; }
        public string PromocodeName { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<Article> Articles { get; set; }
        public List<long> ArticleIds { get; set; }
    }
}
