namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskDateDeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "DateDeleted", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "DateDeleted");
        }
    }
}
