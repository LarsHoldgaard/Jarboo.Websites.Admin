namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Task_ForcedPriority : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "ForcedPriority", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "ForcedPriority");
        }
    }
}
