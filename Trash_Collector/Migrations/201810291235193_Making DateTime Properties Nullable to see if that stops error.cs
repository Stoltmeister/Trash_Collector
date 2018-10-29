namespace Trash_Collector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingDateTimePropertiesNullabletoseeifthatstopserror : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "WeeklyPickupDay", c => c.Int());
            AlterColumn("dbo.Customers", "SpecialPickupDay", c => c.DateTime());
            AlterColumn("dbo.Customers", "PickupPauseDate", c => c.DateTime());
            AlterColumn("dbo.Customers", "ResumePickupDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "ResumePickupDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Customers", "PickupPauseDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Customers", "SpecialPickupDay", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Customers", "WeeklyPickupDay", c => c.Int(nullable: false));
        }
    }
}
