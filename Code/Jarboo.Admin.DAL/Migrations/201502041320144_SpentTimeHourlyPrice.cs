namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpentTimeHourlyPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpentTimes", "HourlyPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpentTimes", "HourlyPrice");
        }
    }
}
