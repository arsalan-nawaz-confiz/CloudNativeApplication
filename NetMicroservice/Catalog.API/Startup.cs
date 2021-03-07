using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Data.Interfaces;
using Catalog.API.Repositories;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services;
using Catalog.API.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Metrics;
using Steeltoe.Management.Endpoint.ThreadDump;
using Steeltoe.Management.Tracing;

namespace Catalog.API
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
            services.AddControllers();
            services.AddHttpClient();
            services.Configure<CatalogDatabaseSettings>(Configuration.GetSection(nameof(CatalogDatabaseSettings)));
            services.AddSingleton<ICatalogDatabaseSettings>(sp => sp.GetRequiredService<IOptions<CatalogDatabaseSettings>>().Value);
            services.AddTransient<ICatalogContext, CatalogContext>();
            services.AddTransient<IProductRepository, ProductRepository>();

            services.AddSwaggerGen(options=> {
                options.SwaggerDoc("v1", new OpenApiInfo {Title="Catalog API", Version="v1" });
            });

            services.AddServiceDiscovery(options => options.UseEureka());

            services.AddSingleton<IHealthContributor, CustomHealthContributor>();
            services.AddHealthActuator(Configuration);

            services.AddMetricsActuator(Configuration);

            services.AddThreadDumpActuator(Configuration);

            // Add Distributed tracing
            services.AddDistributedTracing(Configuration, builder => builder.UseZipkinWithTraceOptions(services));

            services.AddHystrixCommand<OrderCommand>("OrderGroup", Configuration);

            //added to get Metrics stream
            services.AddHystrixMetricsStream(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API");
            });

            app.UseDiscoveryClient();

            app.UseHystrixRequestContext();
            app.UseHystrixMetricsStream();
        }
    }

    public class CustomHealthContributor : IHealthContributor
    {
        public string Id => "CustomHealthContributor";
        public HealthCheckResult Health()
        {
            var result = new HealthCheckResult
            {
                // this is used as part of the aggregate, it is not directly part of the middleware response
                Status = HealthStatus.UP,
                Description = "This health check does not check anything"
            };
            result.Details.Add("status", HealthStatus.UP.ToString());
            return result;
        }
    }
}
