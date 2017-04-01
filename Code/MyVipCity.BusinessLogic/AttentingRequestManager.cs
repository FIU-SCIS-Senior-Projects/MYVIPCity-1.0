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
using Ninject.Extensions.Logging;

namespace MyVipCity.BusinessLogic {

	public class AttentingRequestManager: AbstractEntityManager, IAttentingRequestManager {

		private readonly IEmailService emailService;

		public AttentingRequestManager(IResolver resolver, IMapper mapper, ILogger logger, IEmailService emailService) : base(resolver, mapper, logger) {
			this.emailService = emailService;
		}

		public async Task<ResultDto<bool>> SubmitRequestAsync(AttendingRequestDto attendingRequestDto) {
			if (attendingRequestDto.Id != 0) {
				var msg = $"SubmitRequest - If of {typeof(AttendingRequestDto).Name} must be 0";
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

			// check business is not null
			if (attendingRequestDto.Business == null) {
				var msg = $"SubmitRequest - Business cannot be null";
				Logger.Error(msg);
				throw new InvalidOperationException(msg);
			}
			// set desired promoter to null
			attendingRequestDto.DesiredPromoter = null;
			// map to model
			var attendingRequest = ToModel<AttendingRequest, AttendingRequestDto>(attendingRequestDto);
			// set user Id
			attendingRequest.UserId = userId;
			// set the desired promoter as the same promoter
			attendingRequest.DesiredPromoter = attendingRequest.Promoter;
			// TODOO Validate date
			attendingRequest.Date = DateTime.Now;
			// get access to the db
			var db = Resolver.Resolve<DbContext>();
			// save the attending request
			db.Set<AttendingRequest>().Add(attendingRequest);
			try {
				await db.SaveChangesAsync();
			}
			catch (Exception e) {
				
			}

			return new ResultDto<bool>(true);
		}

		public ResultDto<bool> SubmitRequest(AttendingRequestDto attendingRequestDto) {
			return SubmitRequestAsync(attendingRequestDto).Result;
		}

		private async Task SendNewAttendingRequestEmailToPromoter(AttendingRequest attendingRequest) {
			var emailModel = new NewAttendingRequestPromoterNotificationEmailModel {
				Email = attendingRequest.Email,
				Name = attendingRequest.Name,
				From = "hello@myvipcity.com",
				Subject = $"New attending request for {attendingRequest.Business.Name}",
				To = attendingRequest.Promoter.Email,
				Message = attendingRequest.Message,
				BusinessName = attendingRequest.Business.Name,
				Date = attendingRequest.Date.ToString(CultureInfo.CurrentCulture),
				FemaleCount = attendingRequest.FemaleCount,
				MaleCount = attendingRequest.MaleCount,
				PartyCount = attendingRequest.PartyCount,
				Phone = attendingRequest.ContactNumber,
				PromoterName = attendingRequest.Promoter.FirstName

			};

			await emailService.SendAttendigRequestNotificationToPromoter(emailModel);
		}
	}
}
