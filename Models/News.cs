using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTracker.Models
{
    public class News
    {
        [Key]
        public int New_id {  get; set; }
        [ForeignKey("Cryptocurrencies")]
        public int Crypto_id { get; set; }

        public string Topic { get; set; }

        public string Content { get; set; }

        public DateTime Created_at { get; set; }

        public virtual CryptoCurrencies Cryptocurrencies { get; set; }
    }


    public class NewsDto
    {
     
        public int New_id { get; set; }
  
        public int Crypto_id { get; set; }

        public string Crypto_Symbol { get; set; }

        public string Topic { get; set; }

        public string Content { get; set; }

        public DateTime Created_at { get; set; }

    }

}