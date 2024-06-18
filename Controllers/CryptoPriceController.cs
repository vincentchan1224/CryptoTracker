using CryptoTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CryptoTracker.Models;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace CryptoTracker.Controllers
{
    public class CryptoPriceController : Controller
    {
        HttpClient client = new HttpClient();
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        // GET: CryptoPrice
        public ActionResult Index()
        {
            return View();
        }

 
            public ActionResult CryptoPriceView(int id)
            {

            string url = "https://localhost:44351/api/CryptoCurrenciesData/findcrypto/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CryptocurrencyDto selectedcrypto = response.Content.ReadAsAsync<CryptocurrencyDto>().Result;

            string url2 = "https://localhost:44351/api/Prices/GetLatestPriceDto/"+ id;
            HttpResponseMessage response2 = client.GetAsync(url2).Result;
            PricesDto selectedprice = response2.Content.ReadAsAsync<PricesDto>().Result;

            var cryptoDto = selectedcrypto;
          
                var priceDto = selectedprice;

                var viewModel = new CryptoPriceViewModel
                {
                    crypto = cryptoDto,
                    price = priceDto
                };

                return View(viewModel);
            
        }
    }
}