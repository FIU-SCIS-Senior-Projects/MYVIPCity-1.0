using System;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
using MyVipCity.Domain;
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

		public void Create(BusinessDto businessDto) {
			try {
				var business = Mapper.Map<Domain.Business>(businessDto);
				DbContext.Set<Business>().Add(business);
				BuildFriendlyIdForBusiness(business);
				DbContext.SaveChanges();
			}
			catch (Exception e) {
				Logger.Error(e.Message + "\n" + e.StackTrace);
			}
		}

		private void BuildFriendlyIdForBusiness(Business business) {
			if (string.IsNullOrWhiteSpace(business.Name))
				throw new InvalidOperationException("Business name must be provided.");
			// compute friendly id
			var friendlyName = string.Join("-", business.Name.Trim().Split(' ').Select(s => s.Trim()));
			// count the number of existing businesses with the same friendly id
			var count = DbContext.Set<Business>().Count(b => b.FriendlyIdBase == friendlyName);
			business.FriendlyIdBase = friendlyName;
			business.FriendlyId = friendlyName + (count > 0 ? count.ToString() : "");
		}
	}
}
