namespace AgencySystemDotNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_issue_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PostQuestionViewModelRs", "Answer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PostQuestionViewModelRs", "Answer");
        }
    }
}
