﻿using ApiFormat.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SplitListWebApi.Areas.Identity;
using SplitListWebApi.Areas.Identity.Data;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace SplitListWebApi.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SplitListContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SplitListWebApiContextConnection")));

                services.AddDefaultIdentity<UserModel>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<SplitListContext>();
            });
        }
    }
}