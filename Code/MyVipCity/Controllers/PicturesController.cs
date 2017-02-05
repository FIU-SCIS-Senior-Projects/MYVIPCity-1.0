using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
		public async Task<IHttpActionResult> UploadImage() {
			var imageContentTypes = new List<string> {
				"image/png",
				"image/gif",
				"image/ief",
				"image/jpeg"
			};
			return await Upload(imageContentTypes);
		}

		private async Task<IHttpActionResult> Upload() {
			return await Upload(null);
		}

		private async Task<IHttpActionResult> Upload(List<string> allowedContentTypes) {
			// make sure the request is a multipart/form-data
			if (!Request.Content.IsMimeMultipartContent())
				return StatusCode(HttpStatusCode.UnsupportedMediaType);
			// get temporal directory
			var tempDir = Path.GetTempPath();
			// create a file stream provider
			var provider = new MultipartFormDataStreamProvider(tempDir);
			return await Request.Content
				.ReadAsMultipartAsync(provider)
				.ContinueWith<IHttpActionResult>(
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
						// check if there is a list of allowed content types
						if (allowedContentTypes != null) {
							// check if there is a file data whose content type is not in the allowed list
							if (task.Result.FileData.Any(multipartFileData => !allowedContentTypes.Contains(multipartFileData.Headers.ContentType.MediaType))) {
								return StatusCode(HttpStatusCode.UnsupportedMediaType);
							}
						}
						var loopIndex = 0;
						// create tasks to process the file data
						var tasks = new Task<UploadedFileDto>[task.Result.FileData.Count];
						foreach (var multipartFileData in task.Result.FileData) {
							tasks[loopIndex++] = Task<UploadedFileDto>.Factory.StartNew(f => ProcessFile((MultipartFileData)f), multipartFileData);
						}
						return Task<IHttpActionResult>.Factory.ContinueWhenAll(tasks, ProcessDataSavedResults).Result;
					});
		}

		private UploadedFileDto ProcessFile(MultipartFileData fileData) {
			var fileName = fileData.Headers.ContentDisposition.FileName.Replace("\\\"", "");
			var createdBy = User.Identity.GetUserName();
			var contentType = fileData.Headers.ContentType.MediaType;
			int binaryDataId = -1;
			using (var stream = TryOpenFileStream(fileData.LocalFileName, 5, 200)) {
				// store file in database
				binaryDataId = DbContext.SaveBinaryData(stream, 0, createdBy, contentType);
			}
			File.Delete(fileData.LocalFileName);
			var result = new UploadedFileDto {
				ContentType = contentType,
				FileName = fileName,
				BinaryDataId = binaryDataId
			};
			return result;
		}

		private IHttpActionResult ProcessDataSavedResults(Task<UploadedFileDto>[] tasks) {
			var uploadedFiles = new List<UploadedFileDto>(tasks.Length);
			foreach (var task in tasks) {
				if (task.IsFaulted || task.IsCanceled) {
					// check we have an exception
					if (task.Exception != null) {
						// TODO: log error
						var exception = task.Exception.InnerException ?? task.Exception;
						return InternalServerError(exception);
					}
					return InternalServerError();
				}
				uploadedFiles.Add(task.Result);
			}
			return Ok(uploadedFiles);
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
