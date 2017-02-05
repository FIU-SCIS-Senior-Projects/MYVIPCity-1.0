using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using MyVipCity.Utils;

namespace MyVipCity.Models {

	public class ApplicationDbContext: IdentityDbContext<ApplicationUser> {

		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false) {
		}

		public static ApplicationDbContext Create() {
			return new ApplicationDbContext();
		}

		/// <summary>
		/// Save binary data in the Binaries table.
		/// </summary>
		/// <param name="stream">The stream of data to be saved.</param>
		/// <param name="binaryDataId">If provided value is 0 a new entry will be stored, otherwise the data will be appended to an existing binary data entry.</param>
		/// <param name="createdBy">Specifies the creator.</param>
		/// <param name="contentType">Specifies the content type.</param>
		/// <returns>Returns the id in the database of the saved binary data (-1 if an error occurs).</returns>
		public int SaveBinaryData(Stream stream, int binaryDataId, string createdBy, string contentType) {
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
						// insert the data
						Database.ExecuteSqlCommand("spInsertBinaryData @id OUTPUT, @binaryData, @createdBy, @contentType", idParam, binaryDataParam, createdByParam, contentTypeParam);
						// get the id in the database
						binaryDataId = (int)idParam.Value;
					}
					catch (Exception) {
						transaction.Rollback();
						return -1;
					}
				}
				transaction.Commit();
			}
			return binaryDataId;
		}

		/// <summary>
		/// Retrieves the binary data with the specified id in the form of a Stream.
		/// </summary>
		/// <param name="id">Id of the binary data.</param>
		/// <returns>Stream of data.</returns>
		public Stream GetBinaryData(int id) {
			// create a connection to the database
			var connection = new SqlConnection(Database.Connection.ConnectionString);
			// create a new command
			var command = connection.CreateCommand();
			// set command to Stored Procedure
			command.CommandType = CommandType.StoredProcedure;
			// set the text of the command
			command.CommandText = "spReadBinaryData";
			// build the id parameter for the stored procedure
			var idParam = command.CreateParameter();
			idParam.ParameterName = "id";
			idParam.DbType = DbType.Int32;
			idParam.Direction = ParameterDirection.Input;
			idParam.Value = id;
			// add the parameter to the sp command
			command.Parameters.Add(idParam);
			// open the connection
			connection.Open();
			// create a reader with sequential access (to stream data)
			var reader = command.ExecuteReader(CommandBehavior.SequentialAccess);
			// execute read on the reader
			return reader.Read() ? new SqlDataReaderReadOnlyStream(reader, connection, 0) : Stream.Null;
		}
	}
}