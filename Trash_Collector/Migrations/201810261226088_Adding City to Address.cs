namespace Trash_Collector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCitytoAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "City", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "City");
        }
    }
}
