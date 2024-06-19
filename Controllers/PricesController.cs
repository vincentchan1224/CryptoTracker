using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CryptoTracker.Models;

namespace CryptoTracker.Controllers
{
    public class PricesController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PricesController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44351/api/");
        }

        [HttpPost]

        public ActionResult Create(Prices price)
        {
            string url = "https://localhost:44351/api/PricesData/AddPrice";
            string jsonpayload = jss.Serialize(price);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        public ActionResult New()
        {
            return View();
        }
    }
}
