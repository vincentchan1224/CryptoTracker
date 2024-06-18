namespace CryptoTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pricrsupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prices", "price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prices", "price");
        }
    }
}
