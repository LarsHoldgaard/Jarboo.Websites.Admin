namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldDeliveryNoteToTaskTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "DeliveryNote", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "DeliveryNote");
        }
    }
}
