namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Quizzes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Quizs",
                c => new
                    {
                        QuizId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Link = c.String(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.QuizId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quizs", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Quizs", new[] { "ProjectId" });
            DropTable("dbo.Quizs");
        }
    }
}
