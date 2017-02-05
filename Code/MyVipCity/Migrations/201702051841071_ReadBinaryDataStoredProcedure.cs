namespace MyVipCity.Migrations {
	using System;
	using System.Data.Entity.Migrations;

	public partial class ReadBinaryDataStoredProcedure: DbMigration {
		public override void Up() {
			CreateStoredProcedure(
				"dbo.spReadBinaryData",
				p => new {
					id = p.Int()
				},
				@"	
					SET NOCOUNT ON;

					SELECT B.BinaryData
					FROM [dbo].Binaries B
					WHERE B.Id = @id

					RETURN;
				"
			);
		}

		public override void Down() {
			DropStoredProcedure("dbo.spReadBinaryData");
		}
	}
}
