namespace MyVipCity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BinariesTable : DbMigration
    {
		public override void Up() {
			CreateTable(
				"dbo.Binaries",
				c => new {
					Id = c.Int(nullable: false, identity: true),
					BinaryData = c.Binary(nullable: false),
					CreatedDate = c.DateTimeOffset(nullable: false),
					ContentType = c.String(nullable: true, maxLength: 40)
				})
				.PrimaryKey(t => t.Id);
		}

		public override void Down() {
			DropTable("dbo.Binaries");
		}
	}
}
