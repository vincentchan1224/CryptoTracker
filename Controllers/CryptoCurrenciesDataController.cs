using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CryptoTracker.Models;
using System.Diagnostics;

namespace CryptoTracker.Controllers
{
    public class CryptoCurrenciesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //GET: api/CryptoCurrencies/ListCrypots
        [HttpGet]
        [Route("api/CryptoCurrenciesData/ListCryptos")]
        public IEnumerable<CryptocurrencyDto> ListCryptos()
        {
            List<CryptoCurrencies> cryptocurrencies = db.Cryptocurrencies.ToList();
            List<CryptocurrencyDto> cryptocurrencyDtos = new List<CryptocurrencyDto>();

            cryptocurrencies.ForEach(c => cryptocurrencyDtos.Add(new CryptocurrencyDto()
            {
                Crypto_id = c.Crypto_id,
                Crypto_name = c.Crypto_name,
                Crypto_symbol = c.Crypto_symbol,
                Description = c.Description,
                Market_cap = c.Market_cap,
                Circulating_supply = c.Circulating_supply,
                Max_supply = c.Max_supply,
                Created_at =c.Created_at
            }));

            return cryptocurrencyDtos;
        }
        
        // GET: api/CryptocurrenciesData/FindCrypto/5
        //[ResponseType(typeof(CryptoCurrencies))]
        [HttpGet]
        [Route("api/CryptoCurrenciesData/FindCrypto/{id}")]
        public IHttpActionResult FindCrypto(int id)
        {
            CryptoCurrencies Crypto = db.Cryptocurrencies.Find(id);
            CryptocurrencyDto CryptocurrencyDto = new CryptocurrencyDto()
            {
                Crypto_id = Crypto.Crypto_id,
                Crypto_name = Crypto.Crypto_name,
                Crypto_symbol = Crypto.Crypto_symbol,
                Description = Crypto.Description,
                Market_cap = Crypto.Market_cap,
                Circulating_supply = Crypto.Circulating_supply,
                Max_supply = Crypto.Max_supply,
                Created_at = Crypto.Created_at
            };

            
            if (CryptocurrencyDto == null)
            {
                return NotFound();
            }

            return Ok(CryptocurrencyDto);
        }

        // POST: api/AnimalData/AddAnimal
        [ResponseType(typeof(CryptoCurrencies))]
        [HttpPost]
        public IHttpActionResult AddCrypto(CryptoCurrencies crypto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cryptocurrencies.Add(crypto);
            db.SaveChanges();

            return Ok();
        }


        // POST: api/AnimalData/UpdateAnimal/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/CryptoCurrenciesData/UpdateCrypto/{id}")]
        public IHttpActionResult UpdateCrypto(int id, CryptoCurrencies Crypto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Crypto.Crypto_id)
            {
                Debug.WriteLine("UpdateCrypto: Return BadRequest");
                return BadRequest();
                
            }
            Debug.WriteLine("EntityState.Modified");
            db.Entry(Crypto).State = EntityState.Modified;


            try
            {
                Debug.WriteLine("UpdateCrypto: SaveChanges()");
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CryptoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool CryptoExists(int id)
        {
            return db.Cryptocurrencies.Count(e => e.Crypto_id == id) > 0;
        }






    }
}
