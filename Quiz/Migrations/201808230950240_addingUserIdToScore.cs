namespace Quiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingUserIdToScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Scores", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Scores", "Username", c => c.String(nullable: false));
            CreateIndex("dbo.Scores", "UserId");
            AddForeignKey("dbo.Scores", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Scores", "UserId", "dbo.Users");
            DropIndex("dbo.Scores", new[] { "UserId" });
            AlterColumn("dbo.Scores", "Username", c => c.String());
            DropColumn("dbo.Scores", "UserId");
        }
    }
}
