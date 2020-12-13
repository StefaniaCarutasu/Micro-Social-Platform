namespace MicroSocialPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Birthdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        FriendId = c.Int(nullable: false, identity: true),
                        User1_Id = c.String(maxLength: 128),
                        User2_Id = c.String(maxLength: 128),
                        RequestTime = c.DateTime(nullable: false),
                        Accepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FriendId)
                .ForeignKey("dbo.AspNetUsers", t => t.User1_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User2_Id)
                .Index(t => t.User1_Id)
                .Index(t => t.User2_Id);
            
            AddColumn("dbo.Groups", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "City", c => c.String());
            CreateIndex("dbo.Groups", "ApplicationUser_Id");
            AddForeignKey("dbo.Groups", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friends", "User2_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Friends", "User1_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Groups", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Friends", new[] { "User2_Id" });
            DropIndex("dbo.Friends", new[] { "User1_Id" });
            DropIndex("dbo.Groups", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "City");
            DropColumn("dbo.AspNetUsers", "BirthDate");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.Groups", "ApplicationUser_Id");
            DropTable("dbo.Friends");
        }
    }
}
