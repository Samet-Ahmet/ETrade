using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using WebUI.Helpers;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebUI
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

            services.AddSession();

            services.AddMvc();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login/";
                    options.LogoutPath = "/Account/Logout/";
                    options.AccessDeniedPath = "/Account/AccessDenied/";
                });

            services.AddSingleton<IUserDal, EfUserDal>();
            services.AddSingleton<IUserService, UserManager>();
            services.AddSingleton<IUserRoleDal, EfUserRoleDal>();
            services.AddSingleton<IGenderDal, EfGenderDal>();

            services.AddSingleton<IAuthService, AuthManager>();

            services.AddSingleton<ICategoryDal, EfCategoryDal>();
            services.AddSingleton<ISubCategoryDal, EfSubCategoryDal>();
            services.AddSingleton<ICategoryService, CategoryManager>();

            services.AddSingleton<IWorkerDal, EfWorkerDal>();
            services.AddSingleton<IWorkerService, WorkerManager>();

            services.AddSingleton<ICityDal, EfCityDal>();
            services.AddSingleton<ICityService, CityManager>();
            services.AddSingleton<IDistrictDal, EfDistrictDal>();

            services.AddSingleton<IProductDal, EfProductDal>();
            services.AddSingleton<IProductService, ProductManager>();
            services.AddSingleton<IProductPhotoPathDal, EfProductPhotoPathDal>();

            services.AddSingleton<IBrandDal, EfBrandDal>();
            services.AddSingleton<IBrandService, BrandManager>();

            services.AddSingleton<ICartService, CartManager>();
            services.AddSingleton<ICartSessionHelper, CartSessionHelper>();

            services.AddSingleton<IShippingDetailService, ShippingDetailManager>();
            services.AddSingleton<IShippingDetailDal, EfShippingDetailDal>();

            services.AddSingleton<IOrderDal, EfOrderDal>();
            services.AddSingleton<IOrderDetailDal, EfOrderDetailDal>();
            services.AddSingleton<IOrderService, OrderManager>();
            
            /*
                        services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
                            */
            services.AddControllersWithViews()
                .AddFluentValidation(option =>
                    option.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
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
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Product}/{action=Index}/{id?}");
            });
        }
    }
}
