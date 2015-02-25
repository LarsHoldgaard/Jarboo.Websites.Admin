namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpentTimeDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpentTimes", "Date", c => c.DateTime(nullable: true));

            this.Sql("update dbo.SpentTimes set Date = DateCreated");

            AlterColumn("dbo.SpentTimes", "Accepted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpentTimes", "Date");
        }
    }
}
