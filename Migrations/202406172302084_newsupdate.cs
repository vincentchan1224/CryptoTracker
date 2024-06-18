  namespace CryptoTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "Created_at", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "Created_at");
        }
    }
}
