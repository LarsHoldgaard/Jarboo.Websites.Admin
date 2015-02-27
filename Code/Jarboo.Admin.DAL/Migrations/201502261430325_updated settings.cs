namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedsettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "GoogleTemplatePath", c => c.String());
            AddColumn("dbo.Settings", "GoogleBasePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "GoogleBasePath");
            DropColumn("dbo.Settings", "GoogleTemplatePath");
        }
    }
}
