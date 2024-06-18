using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CryptoTracker.Models;

namespace CryptoTracker.Models
{
    public class CryptoPriceViewModel
    {
        public CryptocurrencyDto crypto { get; set; }
        public PricesDto price { get; set; }
    }
}


