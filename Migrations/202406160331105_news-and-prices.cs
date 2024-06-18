namespace CryptoTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsandprices : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.News", "crypto_id");
            CreateIndex("dbo.Prices", "crypto_id");
            AddForeignKey("dbo.News", "crypto_id", "dbo.Cryptocurrencies", "crypto_id", cascadeDelete: true);
            AddForeignKey("dbo.Prices", "crypto_id", "dbo.Cryptocurrencies", "crypto_id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prices", "crypto_id", "dbo.Cryptocurrencies");
            DropForeignKey("dbo.News", "crypto_id", "dbo.Cryptocurrencies");
            DropIndex("dbo.Prices", new[] { "crypto_id" });
            DropIndex("dbo.News", new[] { "crypto_id" });
        }
    }
}
