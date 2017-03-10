using System;
using System.Linq;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
using MyVipCity.Domain;
using Ninject;
using Ninject.Extensions.Logging;

namespace MyVipCity.BusinessLogic {

	public class BusinessManager: AbstractEntityManager, IBusinessManager {

		[Inject]
		public ILogger Logger
		{
			get;
			set;
		}

		public BusinessDto Create(BusinessDto businessDto) {
			try {
				var business = ToModel<Business, BusinessDto>(businessDto);
				BuildFriendlyIdForBusiness(business);
				DbContext.Set<Business>().Add(business);
				DbContext.SaveChanges();
				var result = ToDto<BusinessDto, Business>(business);
				return result;
			}
			catch (Exception e) {
				Logger.Error(e.Message + "\n" + e.StackTrace);
				return null;
			}
		}

		public BusinessDto Update(BusinessDto businessDto) {
			try {
				var business = ToModel<Business, BusinessDto>(businessDto);
				DbContext.SaveChanges();
				var result = ToDto<BusinessDto, Business>(business);
				return result;
			}
			catch (Exception e) {
				Logger.Error(e.Message + "\n" + e.StackTrace);
				return null;
			}
		}

		public BusinessDto LoadById(int id) {
			var business = DbContext.Set<Business>().Find(id);
			var businessDto = ToDto<BusinessDto, Business>(business);
			return businessDto;
		}

		public BusinessDto LoadByFriendlyId(string friendlyId) {
			var business = DbContext.Set<Business>().FirstOrDefault(b => b.FriendlyId == friendlyId);
			var businessDto = ToDto<BusinessDto, Business>(business);
			return businessDto;
		}

		public BusinessDto[] GetAllBusiness() {
			var allBusiness = DbContext.Set<Business>().ToList();
			var allBusinessDtos = Mapper.Map<BusinessDto[]>(allBusiness);
			return allBusinessDtos;
		}

		public PromoterProfileDto[] GetPromoters(int id) {
			var promoterProfiles = DbContext.Set<PromoterProfile>().Where(p => p.Business.Id == id).ToArray();
			var promoterProfilesDto = Mapper.Map<PromoterProfileDto[]>(promoterProfiles);
			return promoterProfilesDto;
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
