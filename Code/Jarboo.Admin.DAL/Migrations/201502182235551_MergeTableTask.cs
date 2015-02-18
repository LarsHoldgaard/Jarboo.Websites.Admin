namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MergeTableTask : DbMigration
    {
        public override void Up()
        {
          DropColumn("dbo.Tasks", "EstimatePrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "EstimatePrice", c => c.Double(nullable: false));
            DropForeignKey("dbo.Quizs", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Quizs", new[] { "ProjectId" });
            DropIndex("dbo.Customers", new[] { "Name" });
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Tasks", "Description");
            DropColumn("dbo.Tasks", "Deadline");
            DropColumn("dbo.Tasks", "EstimatedPrice");
            DropColumn("dbo.AspNetUsers", "DateLastLogin");
            DropTable("dbo.Quizs");
        }
    }
}
