namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTrello : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Projects", "BoardName");
            DropColumn("dbo.Employees", "TrelloId");
            DropColumn("dbo.Tasks", "CardLink");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "CardLink", c => c.String());
            AddColumn("dbo.Employees", "TrelloId", c => c.String(nullable: false));
            AddColumn("dbo.Projects", "BoardName", c => c.String(nullable: false));
        }
    }
}
