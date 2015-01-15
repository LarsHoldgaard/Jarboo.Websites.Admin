namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Documentations",
                c => new
                    {
                        DocumentationId = c.Int(nullable: false, identity: true),
                        DocumentLink = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentationId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        Size = c.Int(nullable: false),
                        Urgency = c.Int(nullable: false),
                        Done = c.Boolean(nullable: false),
                        FolderLink = c.String(),
                        CardLink = c.String(),
                        ProjectId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.TaskSteps",
                c => new
                    {
                        TaskId = c.Int(nullable: false),
                        Step = c.Int(nullable: false),
                        DateEnd = c.DateTime(),
                        EmployeeId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskId, t.Step })
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        SkypeName = c.String(),
                        TrelloId = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        HourlyPrice = c.Double(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.EmployeePositions",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EmployeeId, t.Position })
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskSteps", "TaskId", "dbo.Tasks");
            DropForeignKey("dbo.TaskSteps", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.EmployeePositions", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Tasks", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Documentations", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "CustomerId", "dbo.Customers");
            DropIndex("dbo.EmployeePositions", new[] { "EmployeeId" });
            DropIndex("dbo.TaskSteps", new[] { "EmployeeId" });
            DropIndex("dbo.TaskSteps", new[] { "TaskId" });
            DropIndex("dbo.Tasks", new[] { "ProjectId" });
            DropIndex("dbo.Documentations", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "CustomerId" });
            DropTable("dbo.EmployeePositions");
            DropTable("dbo.Employees");
            DropTable("dbo.TaskSteps");
            DropTable("dbo.Tasks");
            DropTable("dbo.Documentations");
            DropTable("dbo.Projects");
            DropTable("dbo.Customers");
        }
    }
}
