using System;
using Crypto.Application.Common.Interfaces;
using Crypto.Infrastructure.Files;
using Crypto.Infrastructure.Identity;
using Crypto.Infrastructure.Persistence;
using Crypto.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            //if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            //{
            //    services.AddDbContext<ApplicationDbContext>(options =>
            //        options.UseInMemoryDatabase("CryptoDb"));
            //}
            //else
            //{
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            //}

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "872452688721-htk49pomtv201al22ho2ffjccmorh5cb.apps.googleusercontent.com";
                options.ClientSecret = "-D-xtV4ClIsDaxTSmaFQsOhT";
            });
            
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password Settings
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                // SignIn Settings
                options.SignIn.RequireConfirmedEmail = true;

                // User Setting
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/user/access_denied";
                options.Cookie.Name = "leo-coin";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/user/login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            // services.AddAuthorization(options =>
            // {
            //     options.AddPolicy("FullConfirmed", policy => policy.RequireRole());
            // });

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<INotificationService, NotificationService>();

            return services;
        }
    }
}
