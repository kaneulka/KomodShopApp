using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Models.PromocodeModels
{
    public class ProductValue
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public long CategoryId { get; set; }
    }
    public class ArticleValue
    {
        public long Id { get; set; }
        public string ArticleName { get; set; }
        public string ProductName { get; set; }
    }
}
