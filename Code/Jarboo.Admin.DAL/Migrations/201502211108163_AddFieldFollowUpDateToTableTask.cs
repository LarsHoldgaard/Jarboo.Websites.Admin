namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldFollowUpDateToTableTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "FollowUpDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "FollowUpDate");
        }
    }
}
