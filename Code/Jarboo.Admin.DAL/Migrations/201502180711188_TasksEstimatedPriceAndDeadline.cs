namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TasksEstimatedPriceAndDeadline : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "EstimatedPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Tasks", "Deadline", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "Deadline");
            DropColumn("dbo.Tasks", "EstimatedPrice");
        }
    }
}
