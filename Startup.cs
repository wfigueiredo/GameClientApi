using GameProducer.Domain.Model;
using GameProducer.Interfaces.Clients;
using GameProducer.Interfaces.Clients.Http;
using GameProducer.Interfaces.Services;
using GameProducer.Interfaces.Services.Impl;
using GameProducer.Interfaces.Strategy;
using GameProducer.Interfaces.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;

namespace GameProducer
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
            services.AddHttpContextAccessor();
            services.AddControllers();

            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.UseCamelCasing(true);
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                options.SerializerSettings.Converters.Add(new StringEnumConverter
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                });
            });

            services.AddHttpContextAccessor();

            // HTTP clients
            var IGDBSection = Configuration.GetSection("Integration:IGDB");
            services.AddHttpClient(IGDBSection["ClientName"], client => {
                client.BaseAddress = new Uri(IGDBSection["Host"]);
                client.DefaultRequestHeaders.Add("user-key", IGDBSection["ApiKey"]);
            });

            // strategies (singleton)
            services.AddSingleton<PublishStrategyContext>();
            services.AddSingleton<PublishStrategy, QueuePublishStrategy>();
            services.AddSingleton<PublishStrategy, TopicPublishStrategy>();
            
            // services (singleton)
            services.AddSingleton<IPublisherService, PublisherService>();
            services.AddSingleton<IGameService, GameService>();

            // validators (singleton)
            services.AddSingleton<IValidator<Game>, GameValidator>();
            services.AddSingleton<IValidator<User>, UserValidator>();

            // clients (singleton)
            services.AddSingleton<SNSClient>();
            services.AddSingleton<SQSClient>();
            services.AddSingleton<IGDBClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.Map($"/api/publisher/v1", prefixedApp =>
            {
                prefixedApp.UseRouting();
                prefixedApp.UseAuthorization();
                prefixedApp.UseEndpoints(endpoints => endpoints.MapControllers());
            });
        }
    }
}
