using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.Common;
using MyVipCity.DataTransferObjects;
using MyVipCity.Domain;
using MyVipCity.Mailing.Contracts;
using MyVipCity.Mailing.Contracts.EmailModels;
using Ninject.Extensions.Logging;

namespace MyVipCity.BusinessLogic {

	public class PromoterInvitationManager: AbstractEntityManager, IPromoterInvitationManager {

		public PromoterInvitationManager(IResolver resolver, IMapper mapper, ILogger logger, IEmailService emailService) : base(resolver, mapper, logger) {
			EmailService = emailService;
		}

		public IEmailService EmailService
		{
			get; set;
		}

		public ResultDto<bool> SendPromoterInvitations(PromoterInvitationDto[] invitations, string baseUrl) {
			var db = Resolver.Resolve<DbContext>();
			// dictionary to store the businesses businessFriendlyId -> business
			Dictionary<string, Business> businesses = new Dictionary<string, Business>();
			// final list of invitations to be sent
			var invitationsToSend = new List<PromoterInvitationDto>();
			// list of emails that are either pending and there is a new invitation request OR there is a promoter with that email
			var existingEmails = new HashSet<string>();
			// loop invitations
			foreach (var promoterInvitationDto in invitations) {
				// check if this is a new invitation
				if (promoterInvitationDto.Id == 0) {
					// check if there is already a pending invitation for this email and business
					var existingPendingInvitation = db.Set<PromoterInvitation>().FirstOrDefault(pi => pi.ClubFriendlyId == promoterInvitationDto.ClubFriendlyId && pi.Status == PromoterInvitationStatus.Sent && pi.Email == promoterInvitationDto.Email);
					if (existingPendingInvitation != null) {
						existingEmails.Add(promoterInvitationDto.Email);
						// do not send this invitation
						continue;
					}
				}

				// business corresponding to the invitation
				Business business;
				// try to get business from the dictionary
				businesses.TryGetValue(promoterInvitationDto.ClubFriendlyId, out business);
				// not in the dictionary, then get it from the database
				if (business == null)
					business = db.Set<Business>().FirstOrDefault(b => b.FriendlyId == promoterInvitationDto.ClubFriendlyId);
				// if the club was not found, then return false and log it
				if (business == null) {
					var msg = $"Business with [FriendlyId={promoterInvitationDto.ClubFriendlyId}] not found.";
					Logger.Warn(msg);
					return new ResultDto<bool> {
						Error = true,
						Result = false,
						Messages = new[] { msg }
					};
				}

				// there must not exist a promoter associated to the business with the same email of the invitation
				var promoterWithSameEmail = db.Set<PromoterProfile>().FirstOrDefault(p => p.Business.Id == business.Id && p.Email == promoterInvitationDto.Email);
				if (promoterWithSameEmail != null) {
					existingEmails.Add(promoterInvitationDto.Email);
					// do not send this invitation
					continue;
				}

				// add the club to the dictionary
				businesses[business.FriendlyId] = business;
				// this is definitely an invitation that must be sent
				invitationsToSend.Add(promoterInvitationDto);
			}
			// loop each invitation and send the email
			Parallel.ForEach(invitationsToSend, invitation => {
				var business = businesses[invitation.ClubFriendlyId];
				var emailModel = new PromoterInvitationEmailModel {
					To = invitation.Email,
					Subject = "Invitation to join " + business.Name,
					From = "hello@myvipcity.com",
					ClubName = business.Name,
					Name = invitation.Name,
					AcceptInvitationUrl = string.Format(baseUrl, invitation.ClubFriendlyId)
				};
				EmailService.SendPromoterInvitationEmailAsync(emailModel);
				invitation.SentOn = DateTimeOffset.Now;
				invitation.Status = PromoterInvitationStatusDto.Sent;
			});
			// convert from dto to model
			var modelInvitations = Mapper.Map<PromoterInvitationDto[], PromoterInvitation[]>(invitationsToSend.ToArray());
			// add the new invitations
			db.Set<PromoterInvitation>().AddRange(modelInvitations.Where(i => i.Id == 0));
			// find the existing invitations
			var existingInvitations = modelInvitations.Where(i => i.Id > 0).ToList();
			// add the existing invitations to the EF tracker
			foreach (var existingInvitation in existingInvitations) {
				db.Entry(existingInvitation).State = EntityState.Modified;
			}
			// save the changes
			db.SaveChanges();
			var result =  new ResultDto<bool> {
				Error = false,
				Result = true
			};
			if (existingEmails.Any()) {
				result.Messages = new[] { "Invitation was not sent to the following emails: " + string.Join(", ", existingEmails) + "\nEither a pending invitation or a promoter exists for those emails." };
			}
			return result;
		}

		public PromoterInvitationDto[] GetPendingPromoterInvitations(string businessFriendlyId) {
			var db = Resolver.Resolve<DbContext>();
			var pendingInvitations = db.Set<PromoterInvitation>().Where(i => i.ClubFriendlyId == businessFriendlyId && i.Status == PromoterInvitationStatus.Sent).ToArray();
			var pendingInvitationsDto = Mapper.Map<PromoterInvitation[], PromoterInvitationDto[]>(pendingInvitations);
			return pendingInvitationsDto;
		}

		public void DeletePromoterInvitation(int id) {
			var db = Resolver.Resolve<DbContext>();
			// get the set of promoters
			var promotersInvitations = db.Set<PromoterInvitation>();
			// find the invitation with the given id
			var invitation = promotersInvitations.Find(id);
			// make sure the invitation exists, and if so, remove it
			if (invitation != null)
				promotersInvitations.Remove(invitation);
			// save changes
			db.SaveChanges();
		}

		public PromoterInvitationDto GetPendingInvitation(string businessFriendlyId, string userEmail) {
			var db = Resolver.Resolve<DbContext>();
			var invitation = db.Set<PromoterInvitation>().FirstOrDefault(i => i.Status == PromoterInvitationStatus.Sent && i.ClubFriendlyId == businessFriendlyId && i.Email == userEmail);
			if (invitation == null)
				return null;
			var invitationDto = ToDto<PromoterInvitationDto, PromoterInvitation>(invitation);
			return invitationDto;
		}

		public PromoterProfileDto AcceptInvitation(string businessFriendlyId, string userEmail, string userId) {
			if (string.IsNullOrWhiteSpace(businessFriendlyId))
				throw new ArgumentNullException(nameof(businessFriendlyId));
			if (string.IsNullOrWhiteSpace(userEmail))
				throw new ArgumentNullException(nameof(userEmail));
			var db = Resolver.Resolve<DbContext>();
			// find the invitation
			var invitation = db.Set<PromoterInvitation>().FirstOrDefault(i => i.Status == PromoterInvitationStatus.Sent && i.ClubFriendlyId == businessFriendlyId && i.Email == userEmail);
			// invitation must exists
			if (invitation == null)
				throw new Exception($"Pending invitation for user='{userEmail}' and business='{businessFriendlyId}' not found");
			// find the business
			var business = db.Set<Business>().FirstOrDefault(b => b.FriendlyId == businessFriendlyId);
			// business must exists
			if (business == null)
				throw new Exception($"Business with friendlyId='{businessFriendlyId}' not found");
			// change the status of the invitation to accepted
			invitation.Status = PromoterInvitationStatus.Accepted;
			// try to come up with first name and last name for the promoter
			var nameSplitted = invitation.Name.Split(' ');
			var firstName = nameSplitted[0];
			var lastName = string.Join(" ", nameSplitted.Skip(1));
			// create the promoter profile
			var profile = new PromoterProfile {
				UserId = userId,
				Email = userEmail,
				Business = business,
				FirstName = firstName,
				LastName = lastName,
				CreatedOn = DateTimeOffset.UtcNow
			};
			// add it to the corresponding db set
			db.Set<PromoterProfile>().Add(profile);
			// persist changes
			db.SaveChanges();
			// map to dto
			var profileDto = ToDto<PromoterProfileDto, PromoterProfile>(profile);
			// return result
			return profileDto;
		}
	}
}
