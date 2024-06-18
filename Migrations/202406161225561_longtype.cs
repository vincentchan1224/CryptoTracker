namespace CryptoTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class longtype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cryptocurrencies", "Market_cap", c => c.Long(nullable: false));
            AlterColumn("dbo.Cryptocurrencies", "Circulating_supply", c => c.Long(nullable: false));
            AlterColumn("dbo.Cryptocurrencies", "Max_supply", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cryptocurrencies", "Max_supply", c => c.String());
            AlterColumn("dbo.Cryptocurrencies", "Circulating_supply", c => c.String());
            AlterColumn("dbo.Cryptocurrencies", "Market_cap", c => c.String());
        }
    }
}
