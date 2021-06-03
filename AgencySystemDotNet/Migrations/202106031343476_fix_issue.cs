namespace AgencySystemDotNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_issue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PostQuestionViewModelRs", "IsAnswered", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PostQuestionViewModelRs", "IsAnswered");
        }
    }
}
