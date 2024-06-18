using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CryptoTracker.Models;

namespace CryptoTracker.Models
{
    public class PricesViewModel
    {
        public CryptocurrencyDto Cryptocurrency { get; set; }
        public List<PriceDetail> Prices { get; set; }

        public decimal LatestPrice { get; set; }

        public class PriceDetail
        {
            public int Crypto_id {  get; set; }
            public int PriceId { get; set; }

            public decimal Price { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }

}