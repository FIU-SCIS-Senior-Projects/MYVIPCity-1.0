using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using MyVipCity.DataTransferObjects;
using MyVipCity.Models;
using Ninject.Extensions.Logging;

namespace MyVipCity.Controllers.api {

	[RoutePrefix("api/Pictures")]
	public class PicturesController: ApiController {

		public PicturesController(ILogger logger, ApplicationDbContext dbContext) {
			Logger = logger;
			DbContext = dbContext;
		}

		public ILogger Logger
		{
			get;
			set;
		}

		public ApplicationDbContext DbContext
		{
			get;
			set;
		}

		[HttpPost]
		[Authorize]
		// [EnableCors(origins: "*", headers: "*", methods: "*")]
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

		[HttpGet]
		[Route("{id:int}")]
		public async Task<IHttpActionResult> DownloadImage(int id) {
			// get the stream for the requested picture
			var stream = DbContext.GetBinaryData(id);
			// create a HTTP response
			var result = Request.CreateResponse(HttpStatusCode.OK);
			result.Content = new StreamContent(stream);
			result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
			result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
			result.Content.Headers.ContentLength = stream.Length;
			result.Content.Headers.Expires = DateTimeOffset.Now.AddDays(365);
			result.Headers.CacheControl = new CacheControlHeaderValue {
				Public = true,
				MaxAge = TimeSpan.FromSeconds(31536000)
			};
			return ResponseMessage(result);
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
								// log the exception
								Logger.Error(task.Exception.Message + "\n" + task.Exception.StackTrace);
								// return error
								return InternalServerError(task.Exception.InnerException ?? task.Exception);
							}
							Logger.Error("Unknown error");
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
						return Task<IHttpActionResult>.Factory.ContinueWhenAll(tasks, ProcessSavedDataResults).Result;
					});
		}

		private UploadedFileDto ProcessFile(MultipartFileData fileData) {
			var fileName = fileData.Headers.ContentDisposition.FileName.Replace("\"", "");
			var createdBy = User.Identity.GetUserName();
			var contentType = fileData.Headers.ContentType.MediaType;
			int binaryDataId = 0;
			using (var stream = TryOpenFileStream(fileData.LocalFileName, 5, 200)) {
				// store file in database
				binaryDataId = DbContext.SaveBinaryData(stream, binaryDataId, createdBy, contentType);
			}
			// delete file
			File.Delete(fileData.LocalFileName);
			// return result
			var result = new UploadedFileDto {
				ContentType = contentType,
				FileName = fileName,
				BinaryDataId = binaryDataId
			};
			return result;
		}

		private IHttpActionResult ProcessSavedDataResults(Task<UploadedFileDto>[] tasks) {
			var uploadedFiles = new List<UploadedFileDto>(tasks.Length);
			foreach (var task in tasks) {
				if (task.IsFaulted || task.IsCanceled) {
					// check we have an exception
					if (task.Exception != null) {
						// log exception
						Logger.Error(task.Exception.Message + "\n" + task.Exception.StackTrace);
						var exception = task.Exception.InnerException ?? task.Exception;
						return InternalServerError(exception);
					}
					Logger.Error("Unknown error");
					return InternalServerError();
				}
				uploadedFiles.Add(task.Result);
			}
			return Ok(uploadedFiles);
		}

		private Stream TryOpenFileStream(string fileName, int retries, int delayMilliseconds) {
			while (true) {
				try {
					return new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				}
				catch (IOException ex) {
					Logger.Error(ex.Message + "\n" + ex.StackTrace);
					retries--;
					if (retries == 0)
						throw;
					Thread.Sleep(delayMilliseconds);
				}
			}
		}
	}
}
