namespace Trash_Collector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingboolpickeduptocustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "IsPickedUp", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "IsPickedUp");
        }
    }
}
