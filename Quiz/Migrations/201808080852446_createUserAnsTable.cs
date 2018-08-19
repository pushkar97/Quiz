namespace Quiz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createUserAnsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.QuestionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAns", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserAns", "QuestionId", "dbo.Questions");
            DropIndex("dbo.UserAns", new[] { "QuestionId" });
            DropIndex("dbo.UserAns", new[] { "UserId" });
            DropTable("dbo.UserAns");
        }
    }
}
