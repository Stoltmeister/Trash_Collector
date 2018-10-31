namespace Trash_Collector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastPickupDayaddedinsteadofisPickedup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "LastPickupDay", c => c.DateTime());
            DropColumn("dbo.Customers", "IsPickedUp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "IsPickedUp", c => c.Boolean(nullable: false));
            DropColumn("dbo.Customers", "LastPickupDay");
        }
    }
}
