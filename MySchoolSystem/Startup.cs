using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySchoolSystem.Models;

namespace MySchoolSystem
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
            services.AddControllersWithViews();
            services.AddDbContext<MyAppDbContext>(
                option =>
                    {
                        option.UseSqlServer(Configuration.GetConnectionString("SQLServer"));
                        //option.EnableSensitiveDataLogging();
                    }
                );
            //services.AddDbContext<MyAppDbContext>(
            //   option => option.UseSqlServer(Configuration["SQLServerSecret"]));

            //adding Identity, inject to MyAppDbContext
            services.AddIdentity<CustomIdentityUser, IdentityRole>(options => {
                //here we can configure our own password requirements
                options.Password.RequiredLength = 10;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<MyAppDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/UserAccount/login";
                //ReturnUrl is binded to controller when AccessDenied
                options.AccessDeniedPath = "/UserAccount/AccessDenied";
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //for identity
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //testing nested route; check CourseId required at controller (?)
                //GET: course/1/todos 
                //GET: course/1/todos/1  
                endpoints.MapControllerRoute(
                    name: "todos",
                    pattern: "{controller=Course}/{CourseId}/{action=Todos}/{TodoId?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
