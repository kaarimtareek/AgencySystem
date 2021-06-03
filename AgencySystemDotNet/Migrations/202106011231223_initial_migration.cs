namespace AgencySystemDotNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EditorViewModelRs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Phonenumber = c.String(),
                        Photo = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PostViewModelRs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EditorId = c.Int(nullable: false),
                        StatusId = c.String(),
                        Title = c.String(),
                        Body = c.String(),
                        Photo = c.String(),
                        ViewersNumber = c.Int(nullable: false),
                        DislikesNumber = c.Int(nullable: false),
                        LikesNumber = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        EditorName = c.String(),
                        CategoryName = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        EditorViewModelR_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EditorViewModelRs", t => t.EditorViewModelR_Id)
                .Index(t => t.EditorViewModelR_Id);
            
            CreateTable(
                "dbo.PostQuestionViewModelRs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        Question = c.String(),
                        EditorName = c.String(),
                        CustomerName = c.String(),
                        PostViewModelR_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PostViewModelRs", t => t.PostViewModelR_Id)
                .Index(t => t.PostViewModelR_Id);
            
            CreateTable(
                "dbo.LookupPostCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 50),
                        Description = c.String(maxLength: 100),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EditorId = c.Int(nullable: false),
                        StatusId = c.String(maxLength: 50),
                        Title = c.String(maxLength: 100),
                        Body = c.String(),
                        Photo = c.String(),
                        ViewersNumber = c.Int(nullable: false),
                        DislikesNumber = c.Int(nullable: false),
                        LikesNumber = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LookupPostCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.EditorId, cascadeDelete: true)
                .ForeignKey("dbo.LookupPostStatus", t => t.StatusId)
                .Index(t => t.EditorId)
                .Index(t => t.StatusId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Phonenumber = c.String(nullable: false, maxLength: 50),
                        Photo = c.String(),
                        Role = c.String(nullable: false, maxLength: 50),
                        PasswordHash = c.Binary(nullable: false),
                        Salt = c.Binary(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PostQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        IsAnswered = c.Boolean(nullable: false),
                        Question = c.String(nullable: false),
                        Answer = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.CustomerId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.PostInteractions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        InteractionType = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.CustomerId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.PostViews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.CustomerId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.SavedPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.CustomerId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.LookupPostStatus",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50),
                        Name = c.String(maxLength: 50),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "StatusId", "dbo.LookupPostStatus");
            DropForeignKey("dbo.SavedPosts", "PostId", "dbo.Posts");
            DropForeignKey("dbo.SavedPosts", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.PostViews", "PostId", "dbo.Posts");
            DropForeignKey("dbo.PostViews", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.PostInteractions", "PostId", "dbo.Posts");
            DropForeignKey("dbo.PostInteractions", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.Posts", "EditorId", "dbo.Users");
            DropForeignKey("dbo.PostQuestions", "PostId", "dbo.Posts");
            DropForeignKey("dbo.PostQuestions", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.Posts", "CategoryId", "dbo.LookupPostCategories");
            DropForeignKey("dbo.PostViewModelRs", "EditorViewModelR_Id", "dbo.EditorViewModelRs");
            DropForeignKey("dbo.PostQuestionViewModelRs", "PostViewModelR_Id", "dbo.PostViewModelRs");
            DropIndex("dbo.SavedPosts", new[] { "PostId" });
            DropIndex("dbo.SavedPosts", new[] { "CustomerId" });
            DropIndex("dbo.PostViews", new[] { "PostId" });
            DropIndex("dbo.PostViews", new[] { "CustomerId" });
            DropIndex("dbo.PostInteractions", new[] { "PostId" });
            DropIndex("dbo.PostInteractions", new[] { "CustomerId" });
            DropIndex("dbo.PostQuestions", new[] { "PostId" });
            DropIndex("dbo.PostQuestions", new[] { "CustomerId" });
            DropIndex("dbo.Posts", new[] { "CategoryId" });
            DropIndex("dbo.Posts", new[] { "StatusId" });
            DropIndex("dbo.Posts", new[] { "EditorId" });
            DropIndex("dbo.PostQuestionViewModelRs", new[] { "PostViewModelR_Id" });
            DropIndex("dbo.PostViewModelRs", new[] { "EditorViewModelR_Id" });
            DropTable("dbo.LookupPostStatus");
            DropTable("dbo.SavedPosts");
            DropTable("dbo.PostViews");
            DropTable("dbo.PostInteractions");
            DropTable("dbo.PostQuestions");
            DropTable("dbo.Users");
            DropTable("dbo.Posts");
            DropTable("dbo.LookupPostCategories");
            DropTable("dbo.PostQuestionViewModelRs");
            DropTable("dbo.PostViewModelRs");
            DropTable("dbo.EditorViewModelRs");
        }
    }
}
