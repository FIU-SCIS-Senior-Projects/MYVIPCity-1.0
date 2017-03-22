using System;
using System.Linq;
using System.Linq.Expressions;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
using MyVipCity.DataTransferObjects.Social;
using MyVipCity.Domain;
using MyVipCity.Domain.Social;
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

		public void AddPost(int id, PostDto post) {
			var business = DbContext.Set<Business>().Find(id);
			if (business != null) {
				var postDomain = Mapper.Map<Post>(post);
				postDomain.PostedOn = DateTimeOffset.UtcNow;
				business.Posts.Add(postDomain);
				DbContext.SaveChanges();
			}
		}

		public PostDto[] GetPosts(int id, int top) {
			return GetPosts(id, top, null);
		}

		public PostDto[] GetPosts(int id, int top, int afterPostId) {
			return GetPosts(id, top, p => p.Id < afterPostId);
		}

		private PostDto[] GetPosts(int id, int top, Expression<Func<Post, bool>> whereExpression) {
			var business = DbContext.Set<Business>().Find(id);
			if (business == null)
				return null;
			IQueryable<Post> postsQueryable = business.Posts.AsQueryable();
			if (whereExpression != null)
				postsQueryable = postsQueryable.Where(whereExpression);

			var posts = postsQueryable.OrderByDescending(r => r.Id).Take(top).ToArray();
			var postsDto = Mapper.Map<PostDto[]>(posts);
			return postsDto;
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
