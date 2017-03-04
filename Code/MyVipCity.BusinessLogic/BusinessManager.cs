using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using AutoMapper;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
using MyVipCity.Domain;
using MyVipCity.Domain.Automapper.MappingContext;
using MyVipCity.Mailing.Contracts;
using MyVipCity.Mailing.Contracts.EmailModels;
using Ninject;
using Ninject.Extensions.Logging;

namespace MyVipCity.BusinessLogic {

	public class BusinessManager: IBusinessManager {

		[Inject]
		public ILogger Logger
		{
			get;
			set;
		}

		[Inject]
		public DbContext DbContext
		{
			get;
			set;
		}

		[Inject]
		public IMapper Mapper
		{
			get;
			set;
		}

		[Inject]
		public IEmailService EmailService
		{
			get; set;
		}

		public BusinessDto Create(BusinessDto businessDto) {
			try {
				var business = ToModel(businessDto);
				BuildFriendlyIdForBusiness(business);
				DbContext.Set<Business>().Add(business);
				DbContext.SaveChanges();
				var result = ToDto(business);
				return result;
			}
			catch (Exception e) {
				Logger.Error(e.Message + "\n" + e.StackTrace);
				return null;
			}
		}

		public BusinessDto Update(BusinessDto businessDto) {
			try {
				var business = ToModel(businessDto);
				DbContext.SaveChanges();
				var result = ToDto(business);
				return result;
			}
			catch (Exception e) {
				Logger.Error(e.Message + "\n" + e.StackTrace);
				return null;
			}
		}

		public BusinessDto LoadById(int id) {
			var business = DbContext.Set<Business>().Find(id);
			var businessDto = ToDto(business);
			return businessDto;
		}

		public BusinessDto LoadByFriendlyId(string friendlyId) {
			var business = DbContext.Set<Business>().FirstOrDefault(b => b.FriendlyId == friendlyId);
			var businessDto = ToDto(business);
			return businessDto;
		}

		public BusinessDto[] GetAllBusiness() {
			var allBusiness = DbContext.Set<Business>().ToList();
			var allBusinessDtos = Mapper.Map<BusinessDto[]>(allBusiness);
			return allBusinessDtos;
		}

		public bool SendPromoterInvitations(PromoterInvitationDto[] invitations, string baseUrl) {
			// dictionary to store the name of the businesses businessFriedndlyId -> businessName
			Dictionary<string, string> businessesNames = new Dictionary<string, string>();
			foreach (var promoterInvitationDto in invitations) {
				// check if the club is already in the dictionary
				if (businessesNames.ContainsKey(promoterInvitationDto.ClubFriendlyId))
					continue;
				// since the club is not in the dictionary, look for it in the database
				var business = DbContext.Set<Business>().FirstOrDefault(b => b.FriendlyId == promoterInvitationDto.ClubFriendlyId);
				// if the club was not found, the return false and log it
				if (business == null) {
					Logger.Warn($"Business with [FriendlyId={0}] not found.", promoterInvitationDto.ClubFriendlyId);
					return false;
				}
				// add the club to the dictionary
				businessesNames.Add(business.FriendlyId, business.Name);
			}
			// loop each invitation and send the email
			Parallel.ForEach(invitations, invitation => {
				var businessName = businessesNames[invitation.ClubFriendlyId];
				var emailModel = new PromoterInvitationEmailModel {
					To = invitation.Email,
					Subject = "Invitation to join " + businessName,
					From = "hello@myvipcity.com",
					ClubName = businessName,
					Name = invitation.Name,
					AcceptInvitationUrl = baseUrl + "/accept-invitation/" + invitation.ClubFriendlyId
				};
				EmailService.SendPromoterInvitationEmailAsync(emailModel);
				invitation.SentOn = DateTimeOffset.Now;
				invitation.Status = PromoterInvitationStatusDto.Sent;
			});
			// convert from dto to model
			var modelInvitations = Mapper.Map<PromoterInvitationDto[], PromoterInvitation[]>(invitations);
			// add the new invitations
			DbContext.Set<PromoterInvitation>().AddRange(modelInvitations.Where(i => i.Id == 0));
			// find the existing invitations
			var existingInvitations = modelInvitations.Where(i => i.Id > 0).ToList();
			// add the existing invitations to the EF tracker
			foreach (var existingInvitation in existingInvitations) {
				DbContext.Entry(existingInvitation).State = EntityState.Modified;
			}
			// save the changes
			DbContext.SaveChanges();
			return true;
		}

		public PromoterInvitationDto[] GetPendingPromoterInvitations(string businessFriendlyId) {
			var pendingInvitations = DbContext.Set<PromoterInvitation>().Where(i => i.ClubFriendlyId == businessFriendlyId && i.Status == PromoterInvitationStatus.Sent).ToArray();
			var pendingInvitationsDto = Mapper.Map<PromoterInvitation[], PromoterInvitationDto[]>(pendingInvitations);
			return pendingInvitationsDto;
		}

		public void DeletePromoterInvitation(int id) {
			// get the set of promoters
			var promotersInvitations = DbContext.Set<PromoterInvitation>();
			// find the invitation with the given id
			var invitation = promotersInvitations.Find(id);
			// make sure the invitation exists, and if so, remove it
			if (invitation != null)
				promotersInvitations.Remove(invitation);
			// save changes
			DbContext.SaveChanges();
		}

		private BusinessDto ToDto(Business business) {
			if (business == null)
				return null;
			var businessDto = Mapper.Map<BusinessDto>(business);
			return businessDto;
		}

		private Business ToModel(BusinessDto businessDto) {
			Business business;
			DtoToModelContext context = new DtoToModelContext();
			if (businessDto.Id == 0) {
				business = new Business();
			}
			else {
				business = DbContext.Set<Business>().Find(businessDto.Id);
			}
			business = Mapper.Map<BusinessDto, Business>(businessDto, business,
				opts => {
					opts.Items.Add(typeof(DtoToModelContext).Name, context);
					opts.Items.Add(typeof(DbContext).Name, DbContext);
				});
			return business;
		}

		private void BuildFriendlyIdForBusiness(Business business) {
			if (string.IsNullOrWhiteSpace(business.Name))
				throw new InvalidOperationException("Business name must be provided.");
			// compute friendly id
			var friendlyName = string.Join("-", business.Name.Trim().Split(' ').Select(s => s.Trim().ToLowerInvariant()));
			// count the number of existing businesses with the same friendly id
			var count = DbContext.Set<Business>().Count(b => b.FriendlyIdBase == friendlyName);
			business.FriendlyIdBase = friendlyName;
			business.FriendlyId = friendlyName + (count > 0 ? count.ToString() : "");
		}
	}
}
