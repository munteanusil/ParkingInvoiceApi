﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobiliTreeApi.Repositories;
using MobiliTreeApi.Services;

namespace MobiliTreeApi
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
            services.AddSingleton<ISessionsRepository, SessionsRepositoryFake>();
            services.AddSingleton<ICustomerRepository, CustomerRepositoryFake>();
            services.AddSingleton<IParkingFacilityRepository, ParkingFacilityRepositoryFake>();
            services.AddSingleton<IInvoiceService, InvoiceService>();
            services.AddSingleton<ISessionPricingService, SessionPricingService>();
            services.AddSingleton<ISessionsRepository, SessionsRepositoryFake>();
            services.AddSingleton(FakeData.GetSeedCustomers());
            services.AddSingleton(FakeData.GetSeedServiceProfiles());
            services.AddSingleton(FakeData.GetSeedSessions());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "MobiliTree API",
                    Version = "v1"
                });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MobiliTree API V1");
                c.RoutePrefix = "swagger";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=InvoicesView}/{action=Index}/{id?}");

                endpoints.MapControllers(); // pentru API-uri
            });
        }
    }
}
