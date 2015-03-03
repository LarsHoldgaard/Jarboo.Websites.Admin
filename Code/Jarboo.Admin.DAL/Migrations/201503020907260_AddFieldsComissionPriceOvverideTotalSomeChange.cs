namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsComissionPriceOvverideTotalSomeChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpentTimes", "Price", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SpentTimes", "Commission", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SpentTimes", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Projects", "Commission", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Projects", "PriceOverride", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Customers", "Commission", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Employees", "HourlyPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.SpentTimes", "Hours", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.SpentTimes", "HourlyPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpentTimes", "HourlyPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.SpentTimes", "Hours", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Employees", "HourlyPrice", c => c.Double(nullable: false));
            DropColumn("dbo.Customers", "Commission");
            DropColumn("dbo.Projects", "PriceOverride");
            DropColumn("dbo.Projects", "Commission");
            DropColumn("dbo.SpentTimes", "Total");
            DropColumn("dbo.SpentTimes", "Commission");
            DropColumn("dbo.SpentTimes", "Price");
        }
    }
}
