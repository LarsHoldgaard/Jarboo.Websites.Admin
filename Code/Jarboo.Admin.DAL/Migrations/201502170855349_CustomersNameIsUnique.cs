namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomersNameIsUnique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 450));
            CreateIndex("dbo.Customers", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Customers", new[] { "Name" });
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false));
        }
    }
}
