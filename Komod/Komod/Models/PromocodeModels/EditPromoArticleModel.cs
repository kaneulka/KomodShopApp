using Komod.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.PromocodeModels
{
    public class EditPromoArticleModel
    {
        public Article Article { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }
        public List<Promocode> Promocodes { get; set; }
        public List<PromocodeArticle> PromocodeArticles { get; set; }
    }
}
