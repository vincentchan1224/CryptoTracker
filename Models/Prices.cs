using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTracker.Models
{
    public class Prices
    {
        [Key]
        public int Price_id {  get; set; }
        [ForeignKey("Cryptocurrencies")]
        public int Crypto_id { get; set; }

        public decimal Price { get; set; }

        public DateTime Timestamp { get; set; }

        public virtual CryptoCurrencies Cryptocurrencies { get; set; }
    }

    public class PricesDto
    {

        public int Price_id { get; set; }

        public int Crypto_id { get; set; }

        public decimal Price { get; set; }

        public DateTime Timestamp { get; set; }
    }
}