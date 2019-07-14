using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeerTutor.Models;

[assembly: HostingStartup(typeof(PeerTutor.Areas.Identity.IdentityHostingStartup))]
namespace PeerTutor.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                // AddIdentity adds cookie based authentication
                // Adds scoped classes for things like UserManager, SignInManager, PasswordHashers etc..
                // NOTE: Automatically adds the validated user from a cookie to the HttpContext.User
                services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultUI()
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()

                // Adds a provider that generates unique keys and hashes for things like 
                // forgot password links, phone number verification codes etc...
                .AddDefaultTokenProviders()

                // Adds UserStore and RoleStore from this context
                // That are consumed by the UserManager and RoleManager
                .AddEntityFrameworkStores<AppDbContext>();

                //services.AddDefaultIdentity<ApplicationUser>().AddRoles<IdentityRole>()
                //.AddEntityFrameworkStores<AppDbContext>();
            });
        }
    }
}