﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetApp.Common.Abstractions;
using NetApp.EventBus;
using NetApp.EventBus.Abstractions;
using NetApp.Models;
using NetApp.Repository;
using NetApp.Services.Catalog.Events;
using NetApp.Services.Lib.Extensions;
using Newtonsoft.Json;
using System;

namespace NetApp.Services.Catalog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mysqlConnectionString = Configuration.GetConnectionString("MallDB");
            services.AddDbContext<MallDBContext>(opt =>
            {
                opt.UseMySql(mysqlConnectionString);
            });
            services.AddDbContext<IntegrationEventLogContext>(opt =>
            {
                opt.UseMySql(mysqlConnectionString);
            });
            services.AddScoped<MQMallRepo>();
            services.AddScoped<IListRepo<Product>>(s => s.GetRequiredService<MQMallRepo>());
            services.AddScoped<ITreeRepo<Category>>(s => s.GetRequiredService<MQMallRepo>());
            
            var redisConnectionString = Configuration.GetConnectionString("Redis");
            services.AddDistributedRedisCache(opt =>
            {
                //opt.InstanceName = "CatalogService";
                opt.Configuration = redisConnectionString;
            });

            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromHours(1);
                opt.Cookie.HttpOnly = true;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                });

            services.AddOptions();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });

            services.AddMyIdentityServerAuthentication("http://localhost:5050", "api1");

            //allow Ajax calls 
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:3000","http://localhost:5000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddMyEventBus(Configuration);
            services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();
            services.AddTransient<ProductPriceChangedIntegrationEventHandler>();

            services.AddMySwagger("Catalog API", "v0");

            var container = new ContainerBuilder();
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseCors("default");

            app.UseAuthentication();

            app.UseSession();
            app.UseMvc();

            app.EnableMySwaggerWithUI("Catalog API", "v0");

            app.RegisterConsul(Configuration, lifetime);
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<ProductPriceChangedIntegrationEvent, ProductPriceChangedIntegrationEventHandler>();
        }
    }
}