using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.Common;
using MyVipCity.DataTransferObjects;
using MyVipCity.DataTransferObjects.Search;
using MyVipCity.DataTransferObjects.Social;
using MyVipCity.Domain;
using Ninject.Extensions.Logging;

namespace MyVipCity.BusinessLogic {

	public class BusinessManager: AbstractEntityManager, IBusinessManager {

		public BusinessManager(IResolver resolver, IMapper mapper, ILogger logger, IPostsEntityManager postsEntityManager) : base(resolver, mapper, logger) {
			PostsEntityManager = postsEntityManager;
		}

		public IPostsEntityManager PostsEntityManager
		{
			get;
			set;
		}

		public BusinessDto Create(BusinessDto businessDto) {
			try {
				var db = Resolver.Resolve<DbContext>();
				var business = ToModel<Business, BusinessDto>(businessDto);
				BuildFriendlyIdForBusiness(business);
				db.Set<Business>().Add(business);
				db.SaveChanges();
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
				var db = Resolver.Resolve<DbContext>();
				var business = ToModel<Business, BusinessDto>(businessDto);
				db.SaveChanges();
				var result = ToDto<BusinessDto, Business>(business);
				return result;
			}
			catch (Exception e) {
				Logger.Error(e.Message + "\n" + e.StackTrace);
				return null;
			}
		}

		public BusinessDto LoadById(int id) {
			var db = Resolver.Resolve<DbContext>();
			var business = db.Set<Business>().Find(id);
			var businessDto = ToDto<BusinessDto, Business>(business);
			return businessDto;
		}

		public BusinessDto LoadByFriendlyId(string friendlyId) {
			var db = Resolver.Resolve<DbContext>();
			var business = db.Set<Business>().FirstOrDefault(b => b.FriendlyId == friendlyId);
			var businessDto = ToDto<BusinessDto, Business>(business);
			return businessDto;
		}

		public BusinessDto[] GetAllBusiness() {
			var db = Resolver.Resolve<DbContext>();
			var allBusiness = db.Set<Business>().ToList();
			var allBusinessDtos = Mapper.Map<BusinessDto[]>(allBusiness);
			return allBusinessDtos;
		}

		public PromoterProfileDto[] GetPromoters(int id) {
			var db = Resolver.Resolve<DbContext>();
			var promoterProfiles = db.Set<PromoterProfile>().Where(p => p.Business.Id == id).ToArray();
			var promoterProfilesDto = Mapper.Map<PromoterProfileDto[]>(promoterProfiles);
			return promoterProfilesDto;
		}

		public PostDto AddOrUpdatePost(int id, PostDto postDto) {
			return PostsEntityManager.AddOrUpdatePost<Business>(id, postDto);
		}

		public ResultDto<bool> DeletePost(int id, int postId) {
			return PostsEntityManager.DeletePost<Business>(id, postId);
		}

		public PostDto[] GetPosts(int id, int top) {
			return PostsEntityManager.GetPosts<Business>(id, top);
		}

		public PostDto[] GetPosts(int id, int top, int afterPostId) {
			return PostsEntityManager.GetPosts<Business>(id, top, afterPostId);
		}

		public async Task<BusinessDto[]> SearchAsync(BusinessSearchCriteriaDto searchCriteria) {
			return GetAllBusiness();
		}

		private void BuildFriendlyIdForBusiness(Business business) {
			if (string.IsNullOrWhiteSpace(business.Name))
				throw new InvalidOperationException("Business name must be provided.");
			// compute friendly id
			var friendlyName = string.Join("-", business.Name.Trim().Split(' ').Select(s => s.Trim().ToLowerInvariant()));
			var db = Resolver.Resolve<DbContext>();
			// count the number of existing businesses with the same friendly id
			var count = db.Set<Business>().Count(b => b.FriendlyIdBase == friendlyName);
			business.FriendlyIdBase = friendlyName;
			business.FriendlyId = friendlyName + (count > 0 ? count.ToString() : "");
		}
	}
}
