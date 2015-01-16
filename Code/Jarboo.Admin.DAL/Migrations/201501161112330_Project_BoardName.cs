namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Project_BoardName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "BoardName", c => c.String(nullable: false, defaultValue: ""));

            this.Sql(@"
                update Projects
                set BoardName = (select c.Name from Customers c where c.CustomerId = Projects.CustomerId) + ' tasks'
");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "BoardName");
        }
    }
}
