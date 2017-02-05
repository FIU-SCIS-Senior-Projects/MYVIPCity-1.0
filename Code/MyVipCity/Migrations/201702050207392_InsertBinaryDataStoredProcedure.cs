using System.Net.Mime;

namespace MyVipCity.Migrations {
	using System;
	using System.Data.Entity.Migrations;

	public partial class InsertBinaryDataStoredProcedure: DbMigration {

		public override void Up() {
			CreateStoredProcedure(
				"dbo.spInsertBinaryData",
				p => {
					var id = p.Int(defaultValue: null);
					id.IsOutParameter = true;
					id.DefaultValue = null;
					return new {
						id = id,
						binaryData = p.Binary(),
						createdBy = p.String(maxLength: 512),
						contentType = p.String(maxLength: 40)
					};
				},
				@"	-- SET NOCOUNT ON added to prevent extra result sets from
					-- interfering with SELECT statements.
					SET NOCOUNT ON;
							--Check if we need to insert new entry in the table
					IF(@id IS NULL)
						BEGIN
							INSERT INTO[dbo].[Binaries]([BinaryData], [CreatedBy], [ContentType]) VALUES(@binaryData, @createdBy, @contentType)
							SELECT @id = SCOPE_IDENTITY();
						END
					ELSE
						-- TODO: check if there are no references to the existing binary data
						UPDATE[dbo].[Binaries]
						SET[BinaryData] = [BinaryData] + @binaryData
						WHERE[Id] = @id
					
					SELECT @id;
					RETURN;
				"
			);
		}

		public override void Down() {
			DropStoredProcedure("dbo.spInsertBinaryData");
		}
	}
}
