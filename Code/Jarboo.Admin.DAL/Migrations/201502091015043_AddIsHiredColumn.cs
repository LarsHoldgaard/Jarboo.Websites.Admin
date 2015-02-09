namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsHiredColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "IsHired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "IsHired");
        }
    }
}
