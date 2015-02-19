namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "Description");
        }
    }
}
