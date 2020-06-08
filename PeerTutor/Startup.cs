using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeerTutor.Models;
using Stripe;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication;


namespace PeerTutor
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private IHostEnvironment CurrentEnvironment { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }
        //public Startup(IConfiguration configuration, IHostingEnvironment env)
        //{
        //    Configuration = configuration;

        //    CurrentEnvironment = env;
        //}
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //To allow easy access to userId
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //if (CurrentEnvironment.IsDevelopment())
            //{
            //    services.AddDbContext<AppDbContext>(options =>
            //    options.UseInMemoryDatabase("PeerTutor"));
            //}
            //else
            //{
            //    // Add AppplicationDbContext to DI
            //    services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //}

            // Add AppplicationDbContext to DI
            //services.AddDbContext<AppDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            //Add AppplicationDbContext to DI
            //services.AddDbContext<AppDbContext>(x => x.UseSqlServer("Data Source=LocalDatabase.db"));

            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.UseNetTopologySuite()));

            //options.UseInMemoryDatabase("PeerTutor"));

            // Automatically perform database migration
            //services.BuildServiceProvider().GetService<AppDbContext>().Database.Migrate();

            // Add third-party authentication

            services.AddAuthentication(options =>
            {

            })
            
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["GoogleAppId"];
                googleOptions.ClientSecret = Configuration["GoogleSecret"];
                googleOptions.Events.OnRemoteFailure = (context) =>
                {
                    //context.Response.Redirect("/Identity/Account/Register");
                    context.Response.Redirect("/Identity/Account/Register?FailureMessage=" + UrlEncoder.Default.Encode(context.Failure.Message));
                    context.HandleResponse();
                    return System.Threading.Tasks.Task.CompletedTask;
                };

            })
            .AddTwitter(twitterOptions =>
            {
                twitterOptions.ConsumerKey = Configuration["TwitterAppId"];
                twitterOptions.ConsumerSecret = Configuration["TwitterSecret"];
                twitterOptions.Events.OnRemoteFailure = (context) =>
                {
                    //context.Response.Redirect("/Identity/Account/Register");
                    context.Response.Redirect("/Identity/Account/Register?FailureMessage=" + UrlEncoder.Default.Encode(context.Failure.Message));
                    context.HandleResponse();
                    return System.Threading.Tasks.Task.CompletedTask;
                };
            })
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["FacebookAppId"];
                facebookOptions.AppSecret = Configuration["FacebookSecret"];
                facebookOptions.Events.OnRemoteFailure = (context) =>
                {
                    //context.Response.Redirect("/Identity/Account/Register");
                    context.Response.Redirect("/Identity/Account/Register?FailureMessage=" + UrlEncoder.Default.Encode(context.Failure.Message));
                    context.HandleResponse();
                    return System.Threading.Tasks.Task.CompletedTask;
                };
            });

            //Add Stripe
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];




            services.AddTransient<ICourseRepository, CourseRepository>();
            
            services.AddMvc(options =>
            {
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.EnableEndpointRouting = false;
            }).AddXmlSerializerFormatters();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext dbContext)
        {
            //UpdateDatabase(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                
            }
            // migrate any database changes on startup (includes initial db creation)
            dbContext.Database.Migrate();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseStaticFiles();

            // Setup Identity
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}/{id2?}"
                    );

                //routes.MapRoute(
                //   name: "composite",
                //   template: "{controller=Home}/{action=Index}/{UserId?}/{CourseId?}"
                //   );
            }
            );
        }

        private void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<AppDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
