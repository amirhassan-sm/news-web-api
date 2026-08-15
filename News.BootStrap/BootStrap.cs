using Application.Contrast.QueryService;
using Application.Contrast.QueryServices;
using Application.Contrast.Repository;
using Application.Contrast.Services;
using Application.implementation;
using Infrastructure.News.Ef.Persistance;
using Infrastructure.News.Ef.Persistance.Query;
using Infrastructure.News.Ef.Persistance.Repository;
using Infrastructure.Security.Identity;
using Infrastructure.Security.Identity.FileStorage;
using Infrastructure.Security.Identity.Services;
using Infrastructure.Security.Identity.Token;
using Infrastructure.Security.Idetity.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using News.DomainServiceContract.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace News.BootStrap
{
    public static class BootStrap
    {
        public static void WierUpNewsSystem(this IServiceCollection services, string newsConectionString,
            string securityConectionString,string secretKey , string Issuer ,  string audience)
        {
            services.AddDbContext<NewsContext>(optionsAction => optionsAction.UseSqlServer(newsConectionString));
            services.AddDbContext<SecurityContext>(op => op.UseSqlServer(securityConectionString));

            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsQuery, NewsQuery>();
            services.AddScoped<INewsApplication, NewsApplication>();
            services.AddScoped<IGenerateToken, GenerateToken>();
            services.AddScoped<IUserSevices, UserServices>();
            services.AddScoped<IRoleService, RoleServices>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserProfileStorage, UserProfileStorage>();
            services.AddScoped<IUserRoleServices, UserRoleServices>();
            services.AddScoped<ICategoryNewsQueryService, CategoryNewsQueryService>();
            services.AddScoped<INewsCategoryRepository, NewsCategoryRepository>();
            services.AddScoped<ICategoryNewsApplication, CategoryNewsApplication>();





            services.AddIdentityCore<ApplicationUser>(optionAction =>
            {
                optionAction.Password.RequireDigit = false;
                optionAction.Lockout.MaxFailedAccessAttempts = 10;
                optionAction.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                optionAction.Password.RequiredLength = 8;
                optionAction.Password.RequireUppercase = false;
                optionAction.Password.RequireLowercase = true;
                optionAction.Password.RequireNonAlphanumeric = false;
            }).AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<SecurityContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();



            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(optionsAction =>
    {
        optionsAction.RequireHttpsMetadata = false;
        optionsAction.SaveToken = true;
        
        optionsAction.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = audience ,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.FromMinutes(2),
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role,
        };
        optionsAction.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"[JWT] Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"[JWT] Challenge: {context.Error} - {context.ErrorDescription}");
                return Task.CompletedTask;
            },
        };
    });






        }
    }

}
