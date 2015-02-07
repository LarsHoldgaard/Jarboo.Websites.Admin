namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DateCreated", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AddColumn("dbo.AspNetUsers", "DateModified", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DateModified");
            DropColumn("dbo.AspNetUsers", "DateCreated");
        }
    }
}
