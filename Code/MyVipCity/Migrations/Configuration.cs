using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyVipCity.Models;

namespace MyVipCity.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
			AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = true;
		}

        protected override void Seed(ApplicationDbContext context)
        {
			// create roles
			if (!context.Roles.Any()) {
				CreateRoles(context);
			}
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data. E.g.
			//
			//    context.People.AddOrUpdate(
			//      p => p.FullName,
			//      new Person { FullName = "Andrew Peters" },
			//      new Person { FullName = "Brice Lambson" },
			//      new Person { FullName = "Rowan Miller" }
			//    );
			//
		}

		private void CreateRoles(ApplicationDbContext context) {
			// role store
			var roleStore = new RoleStore<ApplicationRole>(context);
			// role manager
			var managerRole = new RoleManager<ApplicationRole>(roleStore);
			// admin role
			var roleAdminToInsert = new ApplicationRole {
				Name = "Admin"
			};
			managerRole.Create(roleAdminToInsert);
			// promoter role
			var rolePromoterToInsert = new ApplicationRole {
				Name = "Promoter"
			};
			managerRole.Create(rolePromoterToInsert);
			// create roles
			context.SaveChanges();
		}
	}
}
