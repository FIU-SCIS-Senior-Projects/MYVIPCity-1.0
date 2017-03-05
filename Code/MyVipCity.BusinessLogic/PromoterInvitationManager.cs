using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
using MyVipCity.Domain;
using MyVipCity.Mailing.Contracts;
using MyVipCity.Mailing.Contracts.EmailModels;
using Ninject;
using Ninject.Extensions.Logging;

namespace MyVipCity.BusinessLogic {

	public class PromoterInvitationManager: IPromoterInvitationManager {

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
					AcceptInvitationUrl = string.Format(baseUrl, invitation.ClubFriendlyId)
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

		public PromoterInvitationDto GetPendingInvitation(string businessFriendlyId, string userEmail) {
			var invitation = DbContext.Set<PromoterInvitation>().FirstOrDefault(i => i.Status == PromoterInvitationStatus.Sent && i.ClubFriendlyId == businessFriendlyId && i.Email == userEmail);
			if (invitation == null)
				return null;
			var invitationDto = Mapper.Map<PromoterInvitation, PromoterInvitationDto>(invitation);
			return invitationDto;
		}
	}
}
