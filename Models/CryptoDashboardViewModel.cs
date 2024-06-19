using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CryptoTracker.Models;
namespace CryptoTracker.Models
{
    public class CryptoDashboardViewModel
    {
        public List<CryptocurrencyDBDto> CryptosSortedByPrice { get; set; }
        public List<CryptocurrencyDBDto> CryptosSortedByMarketCap { get; set; }
        public List<CryptocurrencyDBDto> CryptosSortedByPriceChange { get; set; }

        public List<CryptocurrencyDBDto> AllCryptoByMarketCap { get; set; }

        public List<NewsDto> NewsList { get; set; }

        public int SearchCryptoId { get; set; }

        public string SearchCryptoSymbol { get; set; }
    }
}