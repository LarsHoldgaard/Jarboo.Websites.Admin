namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpentTimeDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpentTimes", "Date", c => c.DateTime(nullable: false));
            this.Sql("update dbo.SpentTimes set Date = DateCreated");
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpentTimes", "Date");
        }
    }
}
