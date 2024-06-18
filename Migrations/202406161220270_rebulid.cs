namespace CryptoTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rebulid : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.News", new[] { "crypto_id" });
            DropIndex("dbo.Prices", new[] { "crypto_id" });
            AddColumn("dbo.Cryptocurrencies", "Market_cap", c => c.String());
            AddColumn("dbo.Cryptocurrencies", "Circulating_supply", c => c.String());
            AddColumn("dbo.Cryptocurrencies", "Max_supply", c => c.String());
            CreateIndex("dbo.News", "Crypto_id");
            CreateIndex("dbo.Prices", "Crypto_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Prices", new[] { "Crypto_id" });
            DropIndex("dbo.News", new[] { "Crypto_id" });
            DropColumn("dbo.Cryptocurrencies", "Max_supply");
            DropColumn("dbo.Cryptocurrencies", "Circulating_supply");
            DropColumn("dbo.Cryptocurrencies", "Market_cap");
            CreateIndex("dbo.Prices", "crypto_id");
            CreateIndex("dbo.News", "crypto_id");
        }
    }
}
