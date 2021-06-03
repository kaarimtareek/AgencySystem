namespace AgencySystemDotNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_vms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminViewModelRs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Phonenumber = c.String(),
                        Photo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AdminViewModelRs");
        }
    }
}
