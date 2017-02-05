using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using MyVipCity.Models;
using Ninject;

namespace MyVipCity.Controllers {

	[RoutePrefix("api/Pictures")]
	public class PicturesController: ApiController {

		[Inject]
		public ApplicationDbContext DbContext
		{
			get;
			set;
		}

		[HttpPost]
		[Authorize]
		[Route("")]
		public async Task<IHttpActionResult> Upload() {
			// make sure the request is a multipart/form-data
			if (!Request.Content.IsMimeMultipartContent())
				return StatusCode(HttpStatusCode.UnsupportedMediaType);
			// get temporal directory
			var tempDir = Path.GetTempPath();
			// create a file stream provider
			var provider = new MultipartFormDataStreamProvider(tempDir);
			await Request.Content.ReadAsMultipartAsync(provider).ContinueWith<IHttpActionResult>(
				task => {
					// check for task cancelation or exception
					if (task.IsCanceled || task.IsFaulted) {
						// check if there is an exception for the task
						if (task.Exception != null) {
							// TODO: Log the exception
							return InternalServerError(task.Exception.InnerException ?? task.Exception);
						}
						// TODO: Log the fact that there was an unknown error
						return InternalServerError();
					}

					var loopIndex = 0;
					var tasks = new Task[task.Result.FileData.Count];
					foreach (var multipartFileData in task.Result.FileData) {
						tasks[loopIndex++] = Task.Factory.StartNew(f => ProcessFile((MultipartFileData)f), multipartFileData);
					}
					return Task<IHttpActionResult>.Factory.ContinueWhenAll(tasks, tasks1 => { return Ok(); }).Result;
				});
			return Ok();
		}

		private void ProcessFile(MultipartFileData fileData) {
			var fileName = fileData.Headers.ContentDisposition.FileName.Replace("\\", "");
			var createdBy = User.Identity.GetUserName();
			using (var stream = TryOpenFileStream(fileData.LocalFileName, 5, 200)) {
				// store file in database
				DbContext.SaveBinaryData(stream, 0, createdBy, fileData.Headers.ContentType.MediaType);
			}
			File.Delete(fileData.LocalFileName);
		}

		private Stream TryOpenFileStream(string fileName, int retries, int delayMilliseconds) {
			// wait for stream to be ready
			while (true) {
				try {
					// create filestream
					return new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				}
				catch (IOException ex) {
					retries--;
					if (retries == 0)
						throw;
					Thread.Sleep(delayMilliseconds);
				}
			}
		}
	}
}
