using GeeekHouseAPI.Data;
using GeeekHouseAPI.Models;
using GeeekHouseAPI.Repository;
using GeeekHouseAPI.Services;
using GeeekHouseAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeeekHouseAPI
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

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<GeekHouseContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option => {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
            var connectionString = Configuration.GetConnectionString("GeekHouseDB");
            services.AddDbContext<GeekHouseContext>(
                options=>options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddScoped<IStorageService, StorageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
         

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }

            app.UseCors(config => {
                config.AllowAnyOrigin();
                config.AllowAnyMethod();
                config.AllowAnyHeader();

            });


            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            CreateUserRoles(services).Wait();

        }

        public async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = {"Admin"};
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleCheck = await roleManager.RoleExistsAsync(roleName);
                if (!roleCheck)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));

                }

            }

            string email = Configuration["AppSettings:UserEmail"];
            string userPassword = Configuration["AppSettings:UserPassword"];
            string firstName = Configuration["AppSettings:FirstName"];
            string lastName = Configuration["AppSettings:LastName"];

            ApplicationUser usuario = await userManager.FindByEmailAsync(email);

            if (usuario == null)
            {
                usuario = new ApplicationUser()
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName
                };
                await userManager.CreateAsync(usuario, userPassword);
            }

            if (!await userManager.IsInRoleAsync(usuario, "Admin"))
            {
                await userManager.AddToRoleAsync(usuario, "Admin");
            }


        }
    }

}

