﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using NetApp.Models;
using NetApp.Workflow;
using NetApp.Workflow.Extensions;

namespace NetApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NetAppDbContext>(opt =>
            {
                opt.UseSqlite("Data Source=workflow.db");
            });

            services.AddSingleton<WorkflowFactory>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";//using a cookie as the primary means to authenticate a use
                options.DefaultChallengeScheme = "oidc";//OpenID Connect scheme
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                //configure the handler that perform the OpenID Connect protocol
                options.SignInScheme = "Cookies";

                options.Authority = "http://localhost:5050";
                options.RequireHttpsMetadata = false;

                options.ClientId = "mvc";
                options.SaveTokens = true;
            });
            
            services.AddHttpClient();

            services.AddMyWorkflow();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            app.UseMyWorkflow(cfg =>
            {
                cfg.RegisterWorkflow<Workflow.Workflows.HelloWorldWorkflow>();
            });
        }
    }
}
