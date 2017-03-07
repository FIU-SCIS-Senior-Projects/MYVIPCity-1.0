﻿using System.Linq;
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
	}
}