using CryptoTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;

namespace CryptoTracker.Controllers
{
    public class PricesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        [Route("api/Prices/GetLatestPrice/{cryptoId}")]
        public IHttpActionResult GetLatestPrice(int cryptoId)
        {
            decimal? price = GetLatestPriceByCryptoId(cryptoId);
            if (price == null)
            {
                return NotFound(); 
            }
            return Ok(price);
        }

        private decimal? GetLatestPriceByCryptoId(int cryptoId)
        {
            var latestPrice = db.Prices
                .Where(p => p.Crypto_id == cryptoId)
                .OrderByDescending(p => p.Timestamp)
                .FirstOrDefault();

            return latestPrice?.Price;
        }

        [HttpGet]
        [Route("api/Prices/GetChange/{cryptoId}")]
        public IHttpActionResult GetChange(int cryptoId)
        {
            decimal? change = GetChangeByCryptoId(cryptoId);
            if (change == null)
            {
                return NotFound();
            }
            return Ok(change);
        }


        private decimal GetChangeByCryptoId(int cryptoId)
        {
            var latestPrice = db.Prices
                .Where(p => p.Crypto_id == cryptoId)
                .OrderByDescending(p => p.Timestamp)
                .FirstOrDefault();

            var beforeLatestPrice = db.Prices
                .Where(p => p.Crypto_id == cryptoId)
                .OrderByDescending(p => p.Timestamp)
                .Skip(1) // Skip the first latest price
                .FirstOrDefault();

            var change = (latestPrice.Price - beforeLatestPrice.Price) / beforeLatestPrice.Price;

            return change;
        }


        // POST: api/PricesData/AddPrice
        [ResponseType(typeof(Prices))]
        [HttpPost]
        public IHttpActionResult AddPrice(Prices price)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Prices.Add(price);
            db.SaveChanges();

            return Ok();
        }

        // GET: api/PricesData/FindPrice/{id}
        [HttpGet]
        [Route("api/PricesData/FindPrice/{id}")]
        public IHttpActionResult FindPrice(int id)
        {
            Prices price = db.Prices.Find(id);
            PricesDto priceDto = new PricesDto()
            {
                Crypto_id = price.Crypto_id,
                Price_id = price.Price_id,
                Price = price.Price,
                Timestamp = price.Timestamp
            };


            if (priceDto == null)
            {
                return NotFound();
            }

            return Ok(priceDto);
        }

        [HttpGet]
        [Route("api/Prices/GetLatestPriceDto/{cryptoId}")]
        public IHttpActionResult GetLatestPricesDto(int cryptoId)
        {
            PricesDto priceDto = GetLatestPricesDtoByCryptoId(cryptoId);
            if (priceDto == null)
            {
                return NotFound();
            }
            return Ok(priceDto);
        }
        private PricesDto GetLatestPricesDtoByCryptoId(int cryptoId)
        {
            var latestPrice = db.Prices
                .Where(p => p.Crypto_id == cryptoId)
                .OrderByDescending(p => p.Timestamp)
                .Select(p => new PricesDto
                {
                    Crypto_id = p.Crypto_id,
                    Price_id = p.Price_id,
                    Price = p.Price,
                    Timestamp = p.Timestamp,

                })
                .FirstOrDefault();

            return latestPrice;
        }

        [HttpGet]
        [Route("api/Prices/GetAllPrices/{cryptoId}")]
        public IHttpActionResult GetAllPrices(int cryptoId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var prices = db.Prices
                           .Where(p => p.Crypto_id == cryptoId)
                           .OrderByDescending(p => p.Timestamp)
                           .ToList();


            db.Configuration.ProxyCreationEnabled = true;
            if (prices == null || !prices.Any())
            {
                return NotFound();
            }

            return Ok(prices);
        }


    }
}

