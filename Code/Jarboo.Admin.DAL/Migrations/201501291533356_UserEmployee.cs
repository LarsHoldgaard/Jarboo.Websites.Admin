namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmployeeId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "EmployeeId");
            AddForeignKey("dbo.AspNetUsers", "EmployeeId", "dbo.Employees", "EmployeeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeId" });
            DropColumn("dbo.AspNetUsers", "EmployeeId");
        }
    }
}
