using System;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.BusinessLogic.Utils;
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

		public async Task<BusinessSearchResultDto> SearchAsync(BusinessSearchCriteriaDto searchCriteria) {
			DbGeography refLocation = null;
			if (searchCriteria.ReferenceLatitude != 0 || searchCriteria.ReferenceLongitude != 0)
				refLocation = DbGeography.FromText($"POINT({searchCriteria.ReferenceLongitude} {searchCriteria.ReferenceLatitude})");

			// prepare top, skip and criteria values
			var top = searchCriteria.Top >= 0 ? searchCriteria.Top : 20;
			var skip = searchCriteria.Skip >= 0 ? searchCriteria.Skip : 0;
			var criteria = searchCriteria.Criteria;

			// get access to the db
			var db = Resolver.Resolve<DbContext>();
			// get the business as an IQueryable
			var allBusiness = db.Set<Business>().AsQueryable();
			// check if there is a text search criteria
			if (!string.IsNullOrWhiteSpace(criteria)) {
				allBusiness = allBusiness.Where(b =>
				b.Name.Contains(criteria) ||
				b.Address.FormattedAddress.Contains(criteria) ||
				b.Address.StateFullName.Contains(criteria) ||
				b.Address.Neighborhood.Contains(criteria) ||
				b.Address.CountryFullName.Contains(criteria));
			}
			// apply reference location sorting
			if (refLocation != null) {
				allBusiness = allBusiness.OrderBy(b => b.Address.Location == null ? 1000000000 : b.Address.Location.Distance(refLocation)).ThenBy(b => b.Id);
			}
			else {
				allBusiness = allBusiness.OrderBy(b => b.Id);
			}

			// apply skip
			allBusiness = allBusiness.Skip(skip);

			// apply take
			allBusiness = allBusiness.Take(top);

			//get results as an array
			var businessList = await allBusiness.ToArrayAsync();

			// set up the result dto
			var result = new BusinessSearchResultDto {
				SearchCriteria = searchCriteria
			};

			if (refLocation != null) {
				try {
					result.DistancesToReferenceCoordinate = businessList.Select(b => GeoUtils.DistanceBetweenCoordinates(refLocation.Latitude.Value, refLocation.Longitude.Value, b.Address.Location.Latitude.Value, b.Address.Location.Longitude.Value)).ToArray();
				}
				catch (Exception exception) {
					Logger.Error(exception, "Exception calculating distances");
				}
			}
			// map the results to dto
			var allBusinessDtos = Mapper.Map<BusinessDto[]>(businessList);

			result.Businesses = allBusinessDtos;

			return result;
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
