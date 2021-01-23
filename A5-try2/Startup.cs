using System;
using System.Collections.Generic;
using System.Linq;
using A5_try2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(A5_try2.Startup))]

namespace A5_try2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoles();
        }
        private void CreateRoles()
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();

                //if (!roleManager.RoleExists("SuperAdmin"))
                //{
                //    role.Name = "SuperAdmin";
                //    roleManager.Create(role);
                //    AddUsers(role.Name);
                //}

                //if (!roleManager.RoleExists("Admin"))
                //{
                //    role.Name = "Admin";
                //    roleManager.Create(role);
                //}

            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        private void AddUsers(string roleName)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser();

            user.UserName = "manager@gmail.com";
            user.Email = "manager@gmail.com";
            string password = "Manager123@#";

            var status = UserManager.Create(user, password);

            if (status.Succeeded)
            {
                UserManager.AddToRole(user.Id, roleName);
            }
        }
    }
}
