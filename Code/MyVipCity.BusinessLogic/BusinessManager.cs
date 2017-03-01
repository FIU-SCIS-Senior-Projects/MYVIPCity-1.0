using System;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
using MyVipCity.Domain;
using MyVipCity.Domain.Automapper.MappingContext;
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
