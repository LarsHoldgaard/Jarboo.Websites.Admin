namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTablesCommentsQuestionAnswerAndFieldRoleToSpentsTime : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerId = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        QuestionId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerId)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.QuestionId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false),
                        TaskId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        Headline = c.String(),
                        Message = c.String(),
                        IsRead = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        LastUpdate = c.DateTime(),
                        TaskId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.Tasks", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId);
            
            AddColumn("dbo.SpentTimes", "Role", c => c.String());
            AddColumn("dbo.Tasks", "EstimatePrice", c => c.Double(nullable: false));
            AddColumn("dbo.Tasks", "CurrentPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Questions", "TaskId", "dbo.Tasks");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Comments", "TaskId", "dbo.Tasks");
            DropForeignKey("dbo.Comments", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Questions", new[] { "TaskId" });
            DropIndex("dbo.Comments", new[] { "EmployeeId" });
            DropIndex("dbo.Comments", new[] { "TaskId" });
            DropIndex("dbo.Answers", new[] { "EmployeeId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropColumn("dbo.Tasks", "CurrentPrice");
            DropColumn("dbo.Tasks", "EstimatePrice");
            DropColumn("dbo.SpentTimes", "Role");
            DropTable("dbo.Questions");
            DropTable("dbo.Comments");
            DropTable("dbo.Answers");
        }
    }
}
