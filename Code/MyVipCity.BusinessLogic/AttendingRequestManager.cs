using System;
using System.Data.Entity;
using System.Globalization;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Identity;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.Common;
using MyVipCity.DataTransferObjects;
using MyVipCity.Domain;
using MyVipCity.Mailing.Contracts;
using MyVipCity.Mailing.Contracts.EmailModels;
using MyVipCity.Mailing.Contracts.EmailModels.AttendingRequest;
using Ninject.Extensions.Logging;

namespace MyVipCity.BusinessLogic {

	public class AttendingRequestManager: AbstractEntityManager, IAttendingRequestManager {

		private readonly IEmailService emailService;
		private readonly IUserManager userManager;

		public AttendingRequestManager(IResolver resolver, IMapper mapper, ILogger logger, IEmailService emailService, IUserManager userManager) : base(resolver, mapper, logger) {
			this.emailService = emailService;
			this.userManager = userManager;
		}

		public async Task<ResultDto<bool>> SubmitRequestAsync(AttendingRequestDto attendingRequestDto, string acceptUrl, string assignVipHostUrl) {
			// make sure Id = 0
			if (attendingRequestDto.Id != 0) {
				var msg = $"SubmitRequest - If of {typeof(AttendingRequestDto).Name} must be 0";
				Logger.Error(msg);
				throw new InvalidOperationException(msg);
			}
			// check date is not in the past
			if (attendingRequestDto.Date < DateTime.Today) {
				var msg = $"SubmitRequest - Date cannot be in the past";
				Logger.Error(msg);
				throw new InvalidOperationException(msg);
			}
			// check business is not null
			if (attendingRequestDto.Business == null) {
				var msg = $"SubmitRequest - Business cannot be null";
				Logger.Error(msg);
				throw new InvalidOperationException(msg);
			}
			if (Thread.CurrentPrincipal == null) {
				Logger.Error("There is no current principal");
				throw new InvalidOperationException();
			}
			// get the id of the currently authenticated user
			var userIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
			var userId = userIdentity.GetUserId();

			// set the status of this request to pending
			attendingRequestDto.Status = AttendingRequestStatusDto.Pending;
			// set submited on time
			attendingRequestDto.SubmittedOn = DateTimeOffset.Now;
			// set desired promoter to null
			attendingRequestDto.DesiredPromoter = null;
			// map to model
			var attendingRequest = ToModel<AttendingRequest, AttendingRequestDto>(attendingRequestDto);
			// set user Id
			attendingRequest.UserId = userId;
			// set the desired promoter as the same promoter
			attendingRequest.DesiredPromoter = attendingRequest.Promoter;
			// get access to the db
			var db = Resolver.Resolve<DbContext>();
			// save the attending request
			db.Set<AttendingRequest>().Add(attendingRequest);
			await db.SaveChangesAsync();
			// send notification emails about this attending request 
			await SendEmailsForAttendingRequest(attendingRequest, acceptUrl, assignVipHostUrl);

			return new ResultDto<bool>(true);
		}

		public ResultDto<bool> SubmitRequest(AttendingRequestDto attendingRequestDto, string acceptUrl, string assignVipHostUrl) {
			return SubmitRequestAsync(attendingRequestDto, acceptUrl, assignVipHostUrl).Result;
		}

		public async Task<AttendingRequestDto> GetPendingRequestForPromoterAsync(int attendingRequestId, string promoterUserId) {
			var db = Resolver.Resolve<DbContext>();
			var request = await db.Set<AttendingRequest>().FindAsync(attendingRequestId);
			if (request == null || request.Status != AttendingRequestStatus.Pending || request.Promoter == null || request.Promoter.UserId != promoterUserId)
				return null;
			var requestDto = ToDto<AttendingRequestDto, AttendingRequest>(request);
			return requestDto;
		}

		public AttendingRequestDto GetPendingRequestForPromoter(int attendingRequestId, string promoterUserId) {
			return GetPendingRequestForPromoterAsync(attendingRequestId, promoterUserId).Result;
		}

		public async Task<AttendingRequestDto> GetRequestAsync(int attendingRequestId) {
			var db = Resolver.Resolve<DbContext>();
			var request = await db.Set<AttendingRequest>().FindAsync(attendingRequestId);
			if (request == null)
				return null;
			var requestDto = ToDto<AttendingRequestDto, AttendingRequest>(request);
			return requestDto;
		}

		public AttendingRequestDto GetRequest(int attendingRequestId) {
			return GetRequestAsync(attendingRequestId).Result;
		}

		public async Task<ResultDto<bool>> AssignPromoterToRequestAsync(int attendingRequestId, int promoterId, string acceptUrl) {
			var db = Resolver.Resolve<DbContext>();
			var request = await db.Set<AttendingRequest>().FindAsync(attendingRequestId);
			if (request == null) {
				return new ResultDto<bool>(false) { Error = true, Messages = new[] { $"Request with Id = {attendingRequestId} not found" } };
			}
			var promoter = await db.Set<PromoterProfile>().FindAsync(promoterId);
			if (promoter.Business.Id != request.Business.Id) {
				return new ResultDto<bool>(false) { Error = true, Messages = new[] { $"Promoter Profile with Id = {promoterId} does not belong to the business of the request" } };
			}

			// assign promoter to request
			request.Promoter = promoter;
			// set request to pending
			request.Status = AttendingRequestStatus.Pending;
			// save changes made to the request
			await db.SaveChangesAsync();
			// send notification to assigned promoter
			await SendAttendingRequestEmailToPromoterAsync(request, acceptUrl);

			return new ResultDto<bool>(true);
		}

		public ResultDto<bool> AssignPromoterToRequest(int attendingRequestId, int promoterId, string acceptUrl) {
			return AssignPromoterToRequestAsync(attendingRequestId, promoterId, acceptUrl).Result;
		}

		public async Task<bool> AcceptRequestAsync(int attendingRequestId, string promoterUserId, string promoterProfileUrl) {
			// get access to the db
			var db = Resolver.Resolve<DbContext>();
			// find the specified attending request
			var request = await db.Set<AttendingRequest>().FindAsync(attendingRequestId);
			// check if it is ok to accept this request
			if (request == null || request.Status != AttendingRequestStatus.Pending || request.Promoter == null || request.Promoter.UserId != promoterUserId) {
				var msg = $"Condition to accept attendingRequest with Id = {attendingRequestId} was not met";
				Logger.Error(msg);
				return false;
			}
			// change the status to accepted
			request.Status = AttendingRequestStatus.Accepted;
			// persist changes
			await db.SaveChangesAsync();
			// send email to user notifying the request has been accepted
			await SendEmailForAcceptedRequest(request, promoterProfileUrl);
			// indicate the request was accepted succesfully
			return true;
		}

		public bool AcceptRequest(int attendingRequestId, string promoterUserId, string promoterProfileUrl) {
			return AcceptRequestAsync(attendingRequestId, promoterUserId, promoterProfileUrl).Result;
		}

		public async Task<bool> DeclineByPromoterAsync(int attendingRequestId, string promoterUserId, string assignVipHostUrl) {
			// get access to the db
			var db = Resolver.Resolve<DbContext>();
			// find the specified attending request
			var request = await db.Set<AttendingRequest>().FindAsync(attendingRequestId);
			// check if it is ok to accept this request
			if (request == null || request.Status != AttendingRequestStatus.Pending || request.Promoter == null || request.Promoter.UserId != promoterUserId) {
				var msg = $"Condition to decline attendingRequest with Id = {attendingRequestId} was not met";
				Logger.Error(msg);
				return false;
			}
			request.Status = AttendingRequestStatus.Declined;
			await db.SaveChangesAsync();
			await SendDeclinedAttendingRequestEmailToAdminsAsync(request, assignVipHostUrl);

			return true;
		}

		public bool DeclineByPromoter(int attendingRequestId, string promoterUserId, string assignVipHostUrl) {
			return DeclineByPromoterAsync(attendingRequestId, promoterUserId, assignVipHostUrl).Result;
		}

		public async Task<bool> DeclineByAdminAsync(int attendingRequestId) {
			// get access to the db
			var db = Resolver.Resolve<DbContext>();
			// find the specified attending request
			var request = await db.Set<AttendingRequest>().FindAsync(attendingRequestId);
			if (request == null)
				return false;

			request.Status = AttendingRequestStatus.Declined;
			await db.SaveChangesAsync();

			var emailModel = new AttendingRequestNotificationEmailModel {
				To = request.Email,
				Subject = $"Request for {request.Business.Name} has been declined"
			};

			FillNewAttendingRequestNotificationEmailModel(emailModel, request);

			await emailService.SendDeclinedAttendingRequestNotificationToUserAsync(emailModel);

			return true;
		}

		public bool DeclineByAdmin(int attendingRequestId) {
			return DeclineByAdminAsync(attendingRequestId).Result;
		}

		private async Task SendEmailForAcceptedRequest(AttendingRequest request, string promoterProfileUrl) {
			var adminEmails = userManager.GetAdminsEmail();
			var emailModel = new AcceptedAttendingRequestNotificationEmailModel {
				Name = request.Name,
				From = "hello@myvipcity.com",
				Subject = "Confirmation for " + request.Business.Name,
				To = request.Email,
				BusinessName = request.Business.Name,
				Date = request.Date.ToString("D", CultureInfo.CurrentCulture),
				Bccs = adminEmails,
				PartyCount = request.PartyCount,
				VipHostName = request.Promoter.FullName(),
				VipHostPageLink = string.Format(promoterProfileUrl, request.Promoter.Id)
			};
			await emailService.SendAcceptedAttendingRequestNotificationToUserAsync(emailModel);
		}

		private async Task SendEmailsForAttendingRequest(AttendingRequest attendingRequest, string acceptUrl, string assignVipHostUrl) {
			// check if the request has a promoter associated to it
			if (attendingRequest.Promoter != null)
				await SendAttendingRequestEmailToPromoterAsync(attendingRequest, acceptUrl);
			else
				await SendAttendingRequestEmailToAdminsAsync(attendingRequest, assignVipHostUrl);
		}

		private async Task SendAttendingRequestEmailToAdminsAsync(AttendingRequest attendingRequest, string assignVipHostUrl) {
			var url = string.Format(assignVipHostUrl, attendingRequest.Id);
			var adminEmails = userManager.GetAdminsEmail();
			await Task.Factory.StartNew(() => {
				Parallel.ForEach(adminEmails, async adminEmail => {
					var emailModel = new AttendingRequestAdminNotificationEmailModel {
						AdminName = adminEmail,
						AssignVipHostUrl = url,
						To = adminEmail,
						Subject = $"New attending request for {attendingRequest.Business.Name} without a promoter."
					};

					FillNewAttendingRequestNotificationEmailModel(emailModel, attendingRequest);

					await emailService.SendAttendigRequestNotificationToAdminAsync(emailModel);
				});
			});
		}

		private async Task SendDeclinedAttendingRequestEmailToAdminsAsync(AttendingRequest attendingRequest, string assignVipHostUrl) {
			await Task.Factory.StartNew(() => {
				var url = string.Format(assignVipHostUrl, attendingRequest.Id);
				var adminEmails = userManager.GetAdminsEmail();
				var businessName = attendingRequest.Business.Name;
				Parallel.ForEach(adminEmails, async adminEmail => {
					var emailModel = new DeclinedAttendingRequestAdminNotificationEmailModel {
						AdminName = adminEmail,
						AssignVipHostUrl = url,
						To = adminEmail,
						Subject = $"Attending request for {businessName} has been declined by VIP host {attendingRequest.Promoter.FullName()}",
						VipHostName = attendingRequest.Promoter.FullName()
					};

					FillNewAttendingRequestNotificationEmailModel(emailModel, attendingRequest);

					await emailService.SendDeclinedAttendingRequestNotificationToAdminAsync(emailModel);
				});
			});
		}

		private async Task SendAttendingRequestEmailToPromoterAsync(AttendingRequest attendingRequest, string acceptUrl) {
			var url = string.Format(acceptUrl, attendingRequest.Id);
			var adminEmails = userManager.GetAdminsEmail();
			var emailModel = new AttendingRequestPromoterNotificationEmailModel {
				Name = attendingRequest.Name,
				DeclineLink = url,
				AcceptLink = url,
				Bccs = adminEmails,
				Subject = $"New attending request for {attendingRequest.Business.Name}",
				PromoterName = attendingRequest.Promoter.FirstName,
				To = attendingRequest.Promoter.Email
			};

			FillNewAttendingRequestNotificationEmailModel(emailModel, attendingRequest);

			await emailService.SendAttendigRequestNotificationToPromoterAsync(emailModel);
		}

		private void FillNewAttendingRequestNotificationEmailModel(AttendingRequestNotificationEmailModel model, AttendingRequest attendingRequest) {
			model.Email = attendingRequest.Email;
			model.From = "hello@myvipcity.com";
			model.Message = attendingRequest.Message;
			model.BusinessName = attendingRequest.Business.Name;
			model.Date = attendingRequest.Date.ToString("D", CultureInfo.CurrentCulture);
			model.FemaleCount = attendingRequest.FemaleCount;
			model.MaleCount = attendingRequest.MaleCount;
			model.PartyCount = attendingRequest.PartyCount;
			model.Phone = attendingRequest.ContactNumber;
			model.Service = attendingRequest.DesiredService == AttendingRequestService.PriorityGeneralEntry ? "Priority General Entry" : "VIP Table Service";
		}
	}
}
