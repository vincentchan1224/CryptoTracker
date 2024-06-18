using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CryptoTracker.Models;
using Newtonsoft.Json.Linq;
using static CryptoTracker.Models.PricesViewModel;

namespace CryptoTracker.Controllers
{
    public class CryptoCurrenciesController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private ApplicationDbContext db = new ApplicationDbContext();
        static CryptoCurrenciesController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44351/api/");
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: CryptoCurrencies/List
        public async Task<ActionResult> List()
        {
            //objective: communicate with our crypto data api to retrieve a list of cryptos
            //curl https://localhost:44351/api/CryptoCurrenciesData/listcryptos
            string url = "CryptoCurrenciesData/ListCryptos";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var cryptos = await response.Content.ReadAsAsync<IEnumerable<CryptocurrencyDto>>();
                return View(cryptos);
            }
            else
            {
                // Handle the error or redirect, maybe to an error page or back with an error message
                return RedirectToAction("Error");
            }
        }

        // GET: CryptoCurrencies/Details/{crypto_id}
        public async Task<ActionResult> Details(int id)
        {
            // Fetch the cryptocurrency details from the findcrypto API
            string detailsUrl = "CryptoCurrenciesData/findcrypto/" + id;
            HttpResponseMessage detailsResponse = client.GetAsync(detailsUrl).Result;

            if (!detailsResponse.IsSuccessStatusCode)
            {
                return HttpNotFound("Cryptocurrency not found.");
            }

            var selectedCrypto = detailsResponse.Content.ReadAsAsync<CryptocurrencyDto>().Result;

            // Fetch all prices for this cryptocurrency
            string pricesUrl = "Prices/GetAllPrices/" + id; // Assumes this endpoint returns all prices
            HttpResponseMessage pricesResponse = client.GetAsync(pricesUrl).Result;

            decimal LatestPrice = 0;
            LatestPrice = await GetLatestPrice(id);

            List<PriceDetail> prices = new List<PriceDetail>();
            if (pricesResponse.IsSuccessStatusCode)
            {
                prices = pricesResponse.Content.ReadAsAsync<List<PriceDetail>>().Result;
            }

            var viewModel = new PricesViewModel
            {
                Cryptocurrency = selectedCrypto,
                Prices = prices,
                LatestPrice = LatestPrice
            };

            return View(viewModel);
        }


        // POST: CryptoCurrencies/Create
        [HttpPost]
        public ActionResult Create(CryptoCurrencies Crypto)
        {
            //objective: add a new CryptoCurrencies into our system using the API
            //curl -H "Content-Type:application/json" -d @CryptoCurrencies.json https://localhost:44351/api/CryptoCurrenciesdata/addCrypto 
            string url = "CryptoCurrenciesData/AddCrypto";

            string jsonpayload = jss.Serialize(Crypto);
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


        // GET: CryptoCurrencies/Edit/{Crypto_Id}
        public ActionResult Edit(int id)
        {
            //grab the CryptoCurrencies information

            //objective: communicate with our CryptoCurrencies data api to retrieve one CryptoCurrencies
            //curl https://localhost:44324/api/CryptoCurrenciesdata/findCryptoCurrencies/{id}

            string url = "CryptoCurrenciesData/findcrypto/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CryptocurrencyDto selectedcrypto = response.Content.ReadAsAsync<CryptocurrencyDto>().Result;
    
            return View(selectedcrypto);
        }

        // POST: CryptoCurrencies/Update/{Crypto_Id}
        [HttpPost]
        public ActionResult Update(int id, CryptoCurrencies crypto)
        {
                //Send the request to the API

                string url = "CryptoCurrenciesData/UpdateCrypto/" + id;

                string jsonpayload = jss.Serialize(crypto);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                Debug.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("Error");
                }
            
        }

        public ActionResult Error()
        {
            return View();
        }
        public async Task<ActionResult> Main()
        {   
            string url = "https://localhost:44351/api/CryptoCurrenciesData/ListCryptos";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var cryptos = await response.Content.ReadAsAsync<IEnumerable<CryptocurrencyDto>>();
                return View(cryptos);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public async Task<ActionResult> Index(string searchCryptoSymbol)
        {
            var cryptocurrencies = db.Cryptocurrencies.ToList();
            var cryptoDataTasks = cryptocurrencies.Select(async c => new CryptocurrencyDBDto
            {
                Crypto_id = c.Crypto_id,
                Crypto_name = c.Crypto_name,
                LatestPrice = await GetLatestPrice(c.Crypto_id),
                Market_cap = c.Market_cap,
                PriceChange = await GetPriceChange(c.Crypto_id),
                Circulating_supply = c.Circulating_supply


            });

            var cryptoData = await Task.WhenAll(cryptoDataTasks);

            var sortedByPrice = cryptoData.OrderByDescending(c => c.LatestPrice).ToList();
            var sortedByMarketCap = cryptoData.OrderByDescending(c => c.Market_cap).ToList();
            var sortedByPriceChange = cryptoData.OrderByDescending(c => c.PriceChange).ToList();
            var newsList = new List<NewsDto>();

            int? searchCryptoId = GetCryptoIdBySymbol(searchCryptoSymbol);

            if (searchCryptoId.HasValue)
            {
                newsList = db.News
                               .Where(n => n.Crypto_id == searchCryptoId)
                               .OrderByDescending(n => n.Created_at)
                               .Take(3)
                               .Select(n => new NewsDto
                               {
                                   New_id = n.New_id,
                                   Crypto_id = n.Crypto_id,
                                   Topic = n.Topic,
                                   Content = n.Content.Length > 100 ? n.Content.Substring(0, 100) + "..." : n.Content,
                                   Created_at = n.Created_at
                               }).ToList();
            }
            else
            {
                // Handle the case where no search ID is provided, or show all/default news
                newsList = db.News
                               .OrderByDescending(n => n.Created_at)
                               .Take(3)
                               .Select(n => new NewsDto
                               {
                                   New_id = n.New_id,
                                   Crypto_id = n.Crypto_id,
                                   Topic = n.Topic,
                                   Content = n.Content.Length > 100 ? n.Content.Substring(0, 100) + "..." : n.Content,
                                   Created_at = n.Created_at
                               }).ToList();
            }

            var viewModel = new CryptoDashboardViewModel
            {
                CryptosSortedByPrice = sortedByPrice,
                CryptosSortedByMarketCap = sortedByMarketCap,
                CryptosSortedByPriceChange = sortedByPriceChange,
                NewsList = newsList
            };

            return View(viewModel);
        }

        private async Task<decimal> GetLatestPrice(int cryptoId)
        {
            var response = await client.GetAsync($"Prices/GetLatestPrice/{cryptoId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<decimal>();
            }
            return default;
        }

        private async Task<decimal> GetPriceChange(int cryptoId)
        {
            var response = await client.GetAsync($"Prices/GetChange/{cryptoId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<decimal>();
            }
            return default;
        }
        [HttpGet]
        public ActionResult GetNewsById(int cryptoId)
        {
            var news = db.News
                         .Where(n => n.Crypto_id == cryptoId)
                         .OrderByDescending(n => n.Created_at)
                         .Take(3)
                         .Select(n => new NewsDto
                         {
                             New_id = n.New_id,
                             Crypto_id = n.Crypto_id,
                             Topic = n.Topic,
                             Content = n.Content.Length > 100 ? n.Content.Substring(0, 100) + "..." : n.Content,
                             Created_at = n.Created_at
                         }).ToList();

            return Json(news, JsonRequestBehavior.AllowGet);
        }

        public int? GetCryptoIdBySymbol(string symbol)
        {
            var crypto = db.Cryptocurrencies.FirstOrDefault(c => c.Crypto_symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
            return crypto?.Crypto_id;
        }

        public string GetCryptoSymbolById(int cryptoId)
        {
            var crypto = db.Cryptocurrencies.FirstOrDefault(c => c.Crypto_id == cryptoId);
            return crypto.Crypto_symbol;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}