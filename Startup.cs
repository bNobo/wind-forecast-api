using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using wind_forecast_api.HealthChecks;
using wind_forecast_api.Options;
using wind_forecast_api.Services;

namespace wind_forecast_api
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
            string mongodbConnectionString = Configuration["Mongo:ConnectionString"];

            services.AddCors(options =>
            {
                options.AddPolicy("corsPolicy", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });
            
            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "wind_forecast_api", Version = "v1" });
            });
            
            services.Configure<PushNotificationsOptions>(Configuration.GetSection("PushNotifications"));
            
            services.AddHostedService<PushNotificationsProducer>();
            
            services.AddSingleton<IPushSubscriptionsService, PushSubscriptionsService>();
            
            services.AddSingleton(new MongoClient(mongodbConnectionString));

            services.AddPushServiceClient(options =>
            {
                Configuration.GetSection("PushNotifications").Bind(options);
            });
            
            services.AddHttpClient();

            services.AddHealthChecks()
                .AddCheck<VersionHealthCheck>("version")
                .AddMongoDb(mongodbConnectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "wind_forecast_api v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors("corsPolicy");

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks(Configuration);
            });            
        }
    }
}
