namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerCountryAndCreator : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Country", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("dbo.Customers", "Creator", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Creator");
            DropColumn("dbo.Customers", "Country");
        }
    }
}
