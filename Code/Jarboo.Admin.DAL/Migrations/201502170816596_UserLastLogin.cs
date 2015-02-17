namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserLastLogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DateLastLogin", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DateLastLogin");
        }
    }
}
