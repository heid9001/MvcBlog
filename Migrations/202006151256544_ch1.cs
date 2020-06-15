namespace BlogMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ch1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.articles", "User_Id", "dbo.users");
            AddForeignKey("dbo.articles", "User_Id", "dbo.users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.articles", "User_Id", "dbo.users");
            AddForeignKey("dbo.articles", "User_Id", "dbo.users", "Id");
        }
    }
}
