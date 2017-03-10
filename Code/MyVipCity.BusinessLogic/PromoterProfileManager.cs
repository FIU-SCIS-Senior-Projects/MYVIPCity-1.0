using System;
using System.Data.Entity.Core;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNet.Identity;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
using MyVipCity.Domain;
using Ninject;
using Ninject.Extensions.Logging;

namespace MyVipCity.BusinessLogic {

	public class PromoterProfileManager: AbstractEntityManager, IPromoterProfileManager {

		[Inject]
		public ILogger Logger
		{
			get;
			set;
		}

		public PromoterProfileDto[] GetPromoterProfiles(string userId) {
			var promoterProfiles = DbContext.Set<PromoterProfile>().Where(p => p.UserId == userId).ToArray();
			var promoterProfileDtos = ToDto<PromoterProfileDto[], PromoterProfile[]>(promoterProfiles);
			return promoterProfileDtos;
		}

		public PromoterProfileDto GetProfileById(int id) {
			// find the profile with the given id
			var promoterProfile = DbContext.Set<PromoterProfile>().Find(id);
			// check if the profile was not found
			if (promoterProfile == null) {
				Logger.Warn($"PromoterProfile with id={id} not found");
				return null;
			}
			var promoterProfileDto = ToDto<PromoterProfileDto, PromoterProfile>(promoterProfile);
			return promoterProfileDto;
		}

		public PromoterProfileDto Update(PromoterProfileDto promoterProfileDto) {
			if (promoterProfileDto.Id == 0) {
				Logger.Error("Promoter profile Id cannot be 0");
				throw new InvalidOperationException("Promoter profile Id cannot be 0");
			}

			if (Thread.CurrentPrincipal == null) {
				Logger.Error("There is no current principal");
				throw new InvalidOperationException();
			}

			var userIdentity = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
			var userId = userIdentity.GetUserId();
			// find the promoter profile with the given id
			var promoterProfile = DbContext.Set<PromoterProfile>().Find(promoterProfileDto.Id);
			// check if it does not exist
			if (promoterProfile == null) {
				Logger.Error($"Promoter profile with Id: {promoterProfileDto.Id} not found");
				throw new ObjectNotFoundException($"Promoter profile with Id: {promoterProfileDto.Id} not found");
			}
			// only the user associated to the profile can edit it
			if (promoterProfile.UserId != userId) {
				Logger.Error($"User with id: {userId} tried to edit Promoter Profile with id: {promoterProfile.Id} which is associated to user: {promoterProfile.UserId}");
				throw new InvalidOperationException();
			}
			// convert from dto to model
			var promoterProfileToUpdate = ToModel(promoterProfileDto, promoterProfile);
			// persist changes
			DbContext.SaveChanges();
			// convert back to dto
			var result = ToDto<PromoterProfileDto, PromoterProfile>(promoterProfileToUpdate);

			return result;
		}
	}
}
