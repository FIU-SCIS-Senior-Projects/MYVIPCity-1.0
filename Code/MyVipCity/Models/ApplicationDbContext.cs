using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyVipCity.Models {

	public class ApplicationDbContext: IdentityDbContext<ApplicationUser> {

		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false) {
		}

		public static ApplicationDbContext Create() {
			return new ApplicationDbContext();
		}

		public void SaveBinaryData(Stream stream, int binaryDataId, string createdBy, string contentType) {
			using (var transaction = Database.BeginTransaction()) {
				// 1 MB buffer
				var buffer = new byte[1024 * 1000];
				// to store the number of bytes read from the stream
				int bytesRead;
				// consume the stream
				while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0) {
					// build the params for the store procedure
					var idParam = new SqlParameter {
						ParameterName = "@id",
						SqlDbType = SqlDbType.Int,
						IsNullable = true,
						Direction = ParameterDirection.InputOutput
					};
					var binaryDataParam = new SqlParameter {
						ParameterName = "@binaryData",
						SqlDbType = SqlDbType.VarBinary
					};
					var createdByParam = new SqlParameter {
						ParameterName = "@createdBy",
						Value = createdBy,
						SqlDbType = SqlDbType.NVarChar
					};
					var contentTypeParam = new SqlParameter {
						ParameterName = "@contentType",
						Value = contentType,
						SqlDbType = SqlDbType.NVarChar
					};
					// create a data byte array to store exactly the number of bytes read from the stream
					var dataToInsert = new byte[bytesRead];
					Array.Copy(buffer, 0, dataToInsert, 0, dataToInsert.Length);
					// update the binary data parameter
					binaryDataParam.Value = dataToInsert;
					// set the @id parameter value
					if (binaryDataId == 0)
						idParam.Value = DBNull.Value;
					else
						idParam.Value = binaryDataId;
					// execute stored procedure to insert the binary data
					try {
						Database.ExecuteSqlCommand("spInsertBinaryData @id OUTPUT, @binaryData, @createdBy, @contentType", idParam, binaryDataParam, createdByParam, contentTypeParam);
						binaryDataId = (int)idParam.Value;
					}
					catch (Exception e) {
						transaction.Rollback();
						break;
					}
				}

				transaction.Commit();
			}
		}
	}
}