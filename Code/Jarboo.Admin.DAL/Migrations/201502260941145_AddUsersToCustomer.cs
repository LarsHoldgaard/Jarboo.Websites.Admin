namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsersToCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserCustomerId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "UserCustomerId");
            AddForeignKey("dbo.AspNetUsers", "UserCustomerId", "dbo.Customers", "CustomerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "UserCustomerId", "dbo.Customers");
            DropIndex("dbo.AspNetUsers", new[] { "UserCustomerId" });
            DropColumn("dbo.AspNetUsers", "UserCustomerId");
        }
    }
}
