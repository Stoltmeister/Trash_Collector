namespace Trash_Collector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedemployeepropertiesemailandzipcode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Email", c => c.String());
            AddColumn("dbo.Employees", "ZipCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "ZipCode");
            DropColumn("dbo.Employees", "Email");
        }
    }
}
