namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpentTime : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SpentTimes",
                c => new
                    {
                        TaskId = c.Int(),
                        Step = c.Int(),
                        SpentTimeId = c.Int(nullable: false, identity: true),
                        Hours = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateVerified = c.DateTime(),
                        DateDeleted = c.DateTime(),
                        Accepted = c.Boolean(),
                        EmployeeId = c.Int(nullable: false),
                        ProjectId = c.Int(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SpentTimeId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.TaskSteps", t => new { t.TaskId, t.Step })
                .Index(t => new { t.TaskId, t.Step })
                .Index(t => t.EmployeeId)
                .Index(t => t.ProjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpentTimes", new[] { "TaskId", "Step" }, "dbo.TaskSteps");
            DropForeignKey("dbo.SpentTimes", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.SpentTimes", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.SpentTimes", new[] { "ProjectId" });
            DropIndex("dbo.SpentTimes", new[] { "EmployeeId" });
            DropIndex("dbo.SpentTimes", new[] { "TaskId", "Step" });
            DropTable("dbo.SpentTimes");
        }
    }
}
