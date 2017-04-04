using System;
using System.Collections.Generic;
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
using Ninject.Extensions.Logging;

namespace MyVipCity.BusinessLogic {

	public class AttendingRequestManager: AbstractEntityManager, IAttendingRequestManager {

		private readonly IEmailService emailService;
		private readonly IUserManager userManager;

		public AttendingRequestManager(IResolver resolver, IMapper mapper, ILogger logger, IEmailService emailService, IUserManager userManager) : base(resolver, mapper, logger) {
			this.emailService = emailService;
			this.userManager = userManager;
		}

		public async Task<ResultDto<bool>> SubmitRequestAsync(AttendingRequestDto attendingRequestDto, string acceptUrl) {
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
			await SendEmailsForNewAttendingRequest(attendingRequest, acceptUrl);

			return new ResultDto<bool>(true);
		}

		public ResultDto<bool> SubmitRequest(AttendingRequestDto attendingRequestDto, string acceptUrl) {
			return SubmitRequestAsync(attendingRequestDto, acceptUrl).Result;
		}

		public async Task<AttendingRequestDto> GetPendingRequestForPromoterAsync(int attendingRequestId, string promoterUserId) {
			var db = Resolver.Resolve<DbContext>();
			var request = await db.Set<AttendingRequest>().FindAsync(attendingRequestId);
			if (request?.Status != AttendingRequestStatus.Pending || request.Promoter == null || request.Promoter.UserId != promoterUserId)
				return null;
			var requestDto = ToDto<AttendingRequestDto, AttendingRequest>(request);
			return requestDto;
		}

		public AttendingRequestDto GetPendingRequestForPromoter(int attendingRequestId, string promoterUserId) {
			return GetPendingRequestForPromoterAsync(attendingRequestId, promoterUserId).Result;
		}

		private async Task SendEmailsForNewAttendingRequest(AttendingRequest attendingRequest, string acceptUrl) {
			var adminEmails = userManager.GetAdminsEmail();
			if (attendingRequest.Promoter != null)
				await SendNewAttendingRequestEmailToPromoter(attendingRequest, adminEmails, acceptUrl);
		}

		private async Task SendNewAttendingRequestEmailToPromoter(AttendingRequest attendingRequest, ICollection<string> adminEmails, string acceptUrl) {

			var url = string.Format(acceptUrl, attendingRequest.Id);

			var emailModel = new NewAttendingRequestPromoterNotificationEmailModel {
				Email = attendingRequest.Email,
				Name = attendingRequest.Name,
				From = "hello@myvipcity.com",
				Subject = $"New attending request for {attendingRequest.Business.Name}",
				To = attendingRequest.Promoter.Email,
				Message = attendingRequest.Message,
				BusinessName = attendingRequest.Business.Name,
				Date = attendingRequest.Date.ToString("D", CultureInfo.CurrentCulture),
				FemaleCount = attendingRequest.FemaleCount,
				MaleCount = attendingRequest.MaleCount,
				PartyCount = attendingRequest.PartyCount,
				Phone = attendingRequest.ContactNumber,
				PromoterName = attendingRequest.Promoter.FirstName,
				Bccs = adminEmails,
				DeclineLink = url,
				AcceptLink = url,
				Service = attendingRequest.DesiredService == AttendingRequestService.PriorityGeneralEntry ? "Priority General Entry" : "VIP Table Service"
			};

			await emailService.SendAttendigRequestNotificationToPromoter(emailModel);
		}
	}
}
