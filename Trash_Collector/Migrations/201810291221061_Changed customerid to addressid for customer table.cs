namespace Trash_Collector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changedcustomeridtoaddressidforcustomertable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Customers", name: "CustomerID", newName: "AddressID");
            RenameIndex(table: "dbo.Customers", name: "IX_CustomerID", newName: "IX_AddressID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Customers", name: "IX_AddressID", newName: "IX_CustomerID");
            RenameColumn(table: "dbo.Customers", name: "AddressID", newName: "CustomerID");
        }
    }
}
