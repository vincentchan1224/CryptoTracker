using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTracker.Models
{
    public class CryptoCurrencies
    {
        // The description of a Crypto currency item
        [Key]
        public int Crypto_id { get; set; }

        public string Crypto_name { get; set; }

        public string Crypto_symbol { get; set; }

        public string Description { get; set; }

        public long Market_cap { get; set; }

        public long Circulating_supply { get; set; }

        public long Max_supply { get; set; }

        public DateTime Created_at { get; set; }

        public virtual ICollection<News> News { get; set; }

        public virtual ICollection<Prices> Prices { get; set; }

    }

    public class CryptocurrencyDto
    {
        public int Crypto_id { get; set; }

        public string Crypto_name { get; set; }

        public string Crypto_symbol { get; set; }

        public string Description { get; set; }

        public long Market_cap { get; set; }

        public long Circulating_supply { get; set; }

        public long Max_supply { get; set; }

        public DateTime Created_at { get; set; }


    }
    //For DB=DashBoard
    public class CryptocurrencyDBDto
    {
        public int Crypto_id { get; set; }

        public string Crypto_name { get; set; }

        public string Crypto_symbol { get; set; }

        public string Description { get; set; }

        public long Market_cap { get; set; }

        public long Circulating_supply { get; set; }

        public long Max_supply { get; set; }

        public DateTime Created_at { get; set; }

        public decimal LatestPrice {  get; set; }

        public decimal PriceChange { get; set; }



    }

}