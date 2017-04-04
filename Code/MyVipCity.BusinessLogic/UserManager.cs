using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.Common;
using Ninject.Extensions.Logging;

namespace MyVipCity.BusinessLogic {

	public class UserManager: AbstractEntityManager, IUserManager {

		public UserManager(IResolver resolver, IMapper mapper, ILogger logger) : base(resolver, mapper, logger) {
		}

		public string GetEmail(string userId) {
			var db = Resolver.Resolve<DbContext>();
			var email = db.Database.SqlQuery<string>($"select Email from AspNetUsers where Id='{userId}'").ToArray();
			return email.Length == 0 ? null : email[0];
		}

		public ICollection<string> GetAdminsEmail() {
			var db = Resolver.Resolve<DbContext>();
			var email = db.Database.SqlQuery<string>($"select U.Email from AspNetRoles R inner join AspNetUserRoles UR on R.Id = UR.RoleId inner join AspNetUsers U on U.Id = UR.UserId where R.Name = 'Admin'").ToArray();
			return email;
		}
	}
}
