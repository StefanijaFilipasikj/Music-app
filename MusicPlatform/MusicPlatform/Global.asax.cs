using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using MusicPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MusicPlatform
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SeedRoles();
        }

        private void SeedRoles()
        {
            // Create a new instance of the ApplicationDbContext
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                // Check if the roles already exist, and create them if they don't
                if (!roleManager.RoleExists("Listener"))
                {
                    var role = new IdentityRole("Listener");
                    roleManager.Create(role);
                }

                if (!roleManager.RoleExists("Artist"))
                {
                    var role = new IdentityRole("Artist");
                    roleManager.Create(role);
                }
            }
        }
    }
}
