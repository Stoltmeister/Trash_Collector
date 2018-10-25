namespace Trash_Collector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeperateAddresstableandlinkfromcustomeraddresstocustomerID : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        StreetNumber = c.Int(nullable: false),
                        Street = c.String(),
                        StateID = c.Int(nullable: false),
                        ZipCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerID);
            
            AddColumn("dbo.Customers", "CustomerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Customers", "CustomerID");
            AddForeignKey("dbo.Customers", "CustomerID", "dbo.Addresses", "CustomerID", cascadeDelete: true);
            DropColumn("dbo.Customers", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Address", c => c.String());
            DropForeignKey("dbo.Customers", "CustomerID", "dbo.Addresses");
            DropIndex("dbo.Customers", new[] { "CustomerID" });
            DropColumn("dbo.Customers", "CustomerID");
            DropTable("dbo.Addresses");
        }
    }
}
