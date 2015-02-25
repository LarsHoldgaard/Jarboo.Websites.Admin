namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedsettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Configuration = c.String(),
                        JarbooInfoEmail = c.String(),
                        UseGoogleDrive = c.Boolean(nullable: false),
                        GoogleClientId = c.String(),
                        GoogleClientSecret = c.String(),
                        GoogleRefreshToken = c.String(),
                        GoogleLocalUserId = c.String(),
                        UseMandrill = c.Boolean(nullable: false),
                        MandrillApiKey = c.String(),
                        MandrillFrom = c.String(),
                        MandrillPasswordRecoveryTemplate = c.String(),
                        MandrillNewTaskTemplate = c.String(),
                        MandrillTaskResponsibleChangedNotificationSubject = c.String(),
                        MandrillTaskResponsibleNotificationTemplate = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Settings");
        }
    }
}
