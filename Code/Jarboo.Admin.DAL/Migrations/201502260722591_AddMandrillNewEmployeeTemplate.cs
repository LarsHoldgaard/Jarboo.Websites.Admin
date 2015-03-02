namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMandrillNewEmployeeTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "MandrillNewEmployeeTemplate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "MandrillNewEmployeeTemplate");
        }
    }
}
