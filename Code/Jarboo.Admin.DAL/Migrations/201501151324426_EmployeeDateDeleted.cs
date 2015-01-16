namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeDateDeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "DateDeleted", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "DateDeleted");
        }
    }
}
