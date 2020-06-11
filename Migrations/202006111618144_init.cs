namespace BlogMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.articles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        User_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsAuthenticated = c.Boolean(nullable: false),
                        IdentityKey = c.String(),
                        AuthorizeToken = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.articles", "User_Id", "dbo.users");
            DropIndex("dbo.articles", new[] { "User_Id" });
            DropTable("dbo.users");
            DropTable("dbo.articles");
        }
    }
}
