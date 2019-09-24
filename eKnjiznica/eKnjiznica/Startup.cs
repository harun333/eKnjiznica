using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eKnjiznica.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using eKnjiznica.Data.Repositories;
using eKnjiznica.Models;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using eKnjiznica.Services;

namespace eKnjiznica
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "MYB_COOKIE";
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IMailService, NullMailService>();
        }

        public void  Configure (IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
