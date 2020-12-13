namespace MicroSocialPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Visibility : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Visibility", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Visibility");
        }
    }
}
