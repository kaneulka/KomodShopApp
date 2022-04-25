using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
        
        public async Task<JsonResult> Metrika()
        {
            var url = "https://api-metrika.yandex.net/stat/v1/data?preset=sources_summary&id&id=73361293";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "OAuth 876da07cda164df78d077e536d7d6455");
                //In the next using statement you will initiate the Get Request, use the await keyword so it will execute the using statement in order.
                using (HttpResponseMessage res = await client.GetAsync(url))
                {
                    //Then get the content from the response in the next using statement, then within it you will get the data, and convert it to a c# object.
                    using (HttpContent content = res.Content)
                    {
                        //Now assign your content to your data variable, by converting into a string using the await keyword.
                        var data = await content.ReadAsStringAsync();
                        //If the data isn't null return log convert the data using newtonsoft JObject Parse class method on the data.
                        return Json(data);
                    }
                }
            }            
        }
    }
}