namespace MyVipCity.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyFromFilesToBinaryData : DbMigration
    {
        public override void Up()
        {
			AddForeignKey("Files", "BinaryDataId", "Binaries", "Id");
		}
        
        public override void Down()
        {
			DropForeignKey("Files", "BinaryDataId", "Binaries");
		}
	}
}
