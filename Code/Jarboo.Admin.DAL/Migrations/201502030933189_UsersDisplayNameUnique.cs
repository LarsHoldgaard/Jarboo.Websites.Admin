namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersDisplayNameUnique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "DisplayName", c => c.String(maxLength: 450));
            CreateIndex("dbo.AspNetUsers", "DisplayName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUsers", new[] { "DisplayName" });
            AlterColumn("dbo.AspNetUsers", "DisplayName", c => c.String());
        }
    }
}
