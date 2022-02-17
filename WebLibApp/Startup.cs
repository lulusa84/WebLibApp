using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebLibApp.Models;
using NLog.Extensions.Logging;
using WebLibApp.Security;


namespace WebLibApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
              // services.AddDbContextPool<DbContext>(
              options => options.
              UseSqlServer(Configuration.GetConnectionString("BookDBConnection")));

            // services.AddIdentity<IdentityUser, IdentityRole>(options =>
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
             {
                 options.Password.RequiredLength = 10;
                 options.Password.RequiredUniqueChars = 3;
                 options.Password.RequireNonAlphanumeric = false;

                 options.SignIn.RequireConfirmedEmail = true;
                 options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                 options.Lockout.MaxFailedAccessAttempts = 5;
                 options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

             }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders()
              .AddTokenProvider<CustomEmailConfirmationTokenProvider
              <ApplicationUser>>("CustomEmailConfirmation");

            // Changes token life span of all token tipes
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                 o.TokenLifespan = TimeSpan.FromHours(5));

            // Changes token life span of Confirmation Email
            // token tipe
            services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
                 o.TokenLifespan = TimeSpan.FromDays(3));

            /*services.Configure<IdentityOptions>(options =>
            { ...
               });*/

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.EnableEndpointRouting = false;
            })
            .AddNewtonsoftJson(options =>
              options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            
            /*.AddXmlSerializerFormatters();

              services.AddControllers().AddNewtonsoftJson(options =>
             options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
             );*/
			 
            //google
            services.AddAuthentication()
               .AddGoogle(options =>
               {
                   options.ClientId = "1073587709852-6r5ntt21jadsbednraq8blddpq1s5sot.apps.googleusercontent.com";
                   options.ClientSecret = "VR2a9gZd_nsH2n4rbCgZxoEO";
                   // options.CallbackPath = "";
               })
            .AddFacebook(options =>
             {
                 options.AppId = "2660740554213936";
                 options.AppSecret = "e847dabd73c1bebe6d484eef3d891784";
             });


            /*services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });
*/

            //policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                   policy => policy.RequireClaim("Delete Role"));
                //.RequireRole("SuperUser"));

                options.AddPolicy("EditRolePolicy",
                    policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Admin") &&
                    context.User.HasClaim(claim => claim.Type == "Edit Role"
                    && claim.Value == "true") || context.User.IsInRole("SuperUser")
                     ));

                // options.InvokeHandlersAfterFailure = false;

                /* RequireClaim("Edit Role", "true") &&
                       .RequireRole("SuperUser")) 
                       .RequireClaim("Edit Role")); 
                    if ClaimRole e ClaimValue ("Edit Role","true")
                   "true" if Claim is selected in the UI, 
                   otherwise "false" */
                /* .AddRequirements
                    new ManageAdminRolesAndClaimsRequirement()));*/

                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Admin"));
            });


            services.AddRazorPages();

            //services.AddSingleton<IBookRepository, MockBookRepository>();
            services.AddScoped<IBookRepository, SQLBookRepository>();
            services.AddScoped<IBookManagementRepository, SQLBookManagementRepository>();
            
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
            services.AddSingleton<DataProtectionPurposeStrings>();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                //WithRedirect
                // The default HSTS value is 30 days. 
                // You may want to change this for 
                // production scenarios,
                // see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            //Default Routing
            //app.UseMvcWithDefaultRoute();

            //Mapped routing
            app.UseMvc(routes =>
            {
                routes.MapRoute("defaults", "{controller=Home}/{action=Index}/{id?}");
                
            });

            // Attribute Routing 
            // app.UseMvc();
            /* app.Run(async (context) =>
             {
                await context.Response.WriteAsync("Hello");
             });*/

            //app.UseRouting();

            // app.UseAuthorization();

            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });*/
        }
    }
}
