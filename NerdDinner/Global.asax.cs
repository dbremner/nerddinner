﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NerdDinner
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/fwlink/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected async void Application_Start()
        {
            System.Data.Entity.Database.SetInitializer(new NerdDinner.Models.NerdDinnerContext.DropCreateIfChangeInitializer());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

           await SetupApplicationRoles();
        }

        async Task<bool> SetupApplicationRoles()
        {
            AuthenticationIdentityManager IdentityManager = new AuthenticationIdentityManager(new IdentityStore());

            bool roleExists = await IdentityManager.Roles.RoleExistsAsync("Admin", CancellationToken.None);
            if (roleExists == false)
            {
                var role = new Role("Admin");
                IdentityResult result = await IdentityManager.Roles.CreateRoleAsync(role);
                if (result.Success == false)
                    return false;
            }
            return true;
        }
    }
}
