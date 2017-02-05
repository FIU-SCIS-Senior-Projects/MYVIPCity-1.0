using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace MyVipCity.Utils {

	internal class SqlDataReaderReadOnlyStream: Stream {

		private readonly SqlDataReader sqlDataReader;
		private readonly int columnIndex;
		private readonly long length;
		private bool streamDisposed;
		private long position;
		private long bytesAvailable;
		private readonly SqlConnection sqlConnection;

		public SqlDataReaderReadOnlyStream(SqlDataReader sqlDataReader, SqlConnection sqlConnection, int columnIndex) {
			if (sqlDataReader == null)
				throw new ArgumentNullException(nameof(sqlDataReader));
			if (sqlConnection == null)
				throw new ArgumentNullException(nameof(sqlConnection));
			this.sqlDataReader = sqlDataReader;
			this.columnIndex = columnIndex;
			this.sqlConnection = sqlConnection;
			position = 0L;
			// get the length in bytes of the reader at the specified column index
			length = sqlDataReader.GetBytes(columnIndex, 0, null, 0, 0);
			bytesAvailable = length;
		}

		~SqlDataReaderReadOnlyStream() {
			Dispose(false);
		}

		public override bool CanRead => true;

		public override bool CanSeek => false;

		public override bool CanWrite => false;

		public override long Length => length;
		
		public override long Position
		{
			get { return position; }
			set { }
		}

		public override int Read(byte[] buffer, int offset, int count) {
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (offset < 0)
				throw new ArgumentOutOfRangeException(nameof(offset));
			if (count < 0)
				throw new ArgumentOutOfRangeException(nameof(count));
			// check if sqlDataReader has been streamDisposed
			if (streamDisposed)
				throw new ObjectDisposedException("sqlDataReader");
			try {
				// loop until we have read count bytes
				while (count > 0 && bytesAvailable > 0) {
					// calculate bytes to read
					var bytesToRead = Math.Min(count, bytesAvailable);
					// read from sqlDataReader (returns number of bytes read)
					var bytesRead = sqlDataReader.GetBytes(columnIndex, position, buffer, offset, (int)bytesToRead);
					// update remaining available bytes
					bytesAvailable -= bytesRead;
					// update starting position in the reader
					position += bytesRead;
					// update bytes still pending for reading
					count -= (int)bytesRead;
					// update offset
					offset += (int)bytesRead;
				}
				return offset;
			}
			catch (Exception ex) {
				throw new IOException("Error reading from SQL Data Reader", ex);
			}
		}

		public override void Flush() {
		}

		public override long Seek(long offset, SeekOrigin origin) {
			throw new NotSupportedException();
		}

		public override void SetLength(long value) {
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count) {
			throw new NotSupportedException();
		}

		protected override void Dispose(bool disposing) {
			// check we are disposing
			if (disposing && !streamDisposed) {
				// dispose sqlDataReader
				sqlDataReader.Dispose();
				// dispose sqlConnection
				sqlConnection.Dispose();
				// update flag
				streamDisposed = true;
			}
			// dispose base
			base.Dispose(disposing);
		}
	}
}