﻿using Microsoft.EntityFrameworkCore;
using WWW.DAL;
using WWW.DAL.Interfaces;
using WWW.DAL.Repositories;
using WWW.Service.Implementations;
using WWW.Service.Interfaces;
using WWW.API;
using WWW.Domain.Entity;
using WWW.Jobs;
using WWW.Jobs.Implementations;
using Org.BouncyCastle.Crypto.Tls;
using Hangfire;
using Hangfire.MemoryStorage;
using WWW.Jobs.Workers;

public static class ServicesExtensions
{
    public static void AddMyServices(this WebApplicationBuilder builder)
    {
        IServiceCollection Services = builder.Services;
//                          SQL
        Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
            connectionString: builder.Configuration.GetConnectionString("StoreDatabase")
            ));
//                        Services DB
        Services.AddTransient<ICategoryRepository, CategoryRepository>();
        Services.AddTransient<IArticleRepository, ArticleRepository>();
        Services.AddTransient<IAccountRepository, AccountRepository>();
        Services.AddTransient<IBaseRepository<Tags>, TagRepository>();

        Services.AddTransient<IArticleService, ArticleService>();
        Services.AddTransient<ICategoryService, CategoryService>();
        Services.AddTransient<IAccountService, AccountService>();

//                          API
        Services.AddTransient<IApiRepository<HttpApiRequest>, HttpApiRequest>();
        Services.AddTransient<IApiRepository<RestApiRequest>, RestApiRequest>();


//                          Jobs
        Services.AddTransient<EventApiJob>();

//                          HangFire (Job Schedule)
        builder.Services.AddHostedService<JobWorker>();

        Services.AddSingleton<IJobService, JobService>();

        Services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMemoryStorage()
        );
        Services.AddHangfireServer();

    }
}