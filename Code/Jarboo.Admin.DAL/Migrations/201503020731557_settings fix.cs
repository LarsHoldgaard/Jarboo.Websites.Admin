namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class settingsfix : DbMigration
    {
        public override void Up()
        {
//            AddColumn("dbo.AspNetUsers", "UserCustomerId", c => c.Int());
//            AddColumn("dbo.Settings", "MandrillNewEmployeeTemplate", c => c.String());
//            CreateIndex("dbo.AspNetUsers", "UserCustomerId");
//            AddForeignKey("dbo.AspNetUsers", "UserCustomerId", "dbo.Customers", "CustomerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "UserCustomerId", "dbo.Customers");
            DropIndex("dbo.AspNetUsers", new[] { "UserCustomerId" });
            DropColumn("dbo.Settings", "MandrillNewEmployeeTemplate");
            DropColumn("dbo.AspNetUsers", "UserCustomerId");
        }
    }
}
