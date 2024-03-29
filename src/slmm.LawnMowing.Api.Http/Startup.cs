﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Slmm.WebApplication
{
    using slmm.LawnMowing.Api;
    using slmm.LawnMowing.Api.Factories;
    using slmm.LawnMowing.Api.Service;

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
            services.AddMvc();
            services.AddSingleton<AsyncSmartLawnMowingMachineService>();
            services.AddSingleton<IMowerFactory, MowerFactory>();
            services.AddSingleton<ISettingsResolver>(new ConfigurationSettingsResolver(Configuration));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Smart Lawn Mowing Machine [L]", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new
                    List<string> { "/swagger" }
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Lawn Mowing Machine V1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
