using GameProducer.Domain.Model;
using GameProducer.Infrastructure.Contracts;
using GameProducer.Infrastructure.Extensions;
using GameProducer.Infrastructure.Security;
using GameProducer.Infrastructure.Security.Impl;
using GameProducer.Interfaces.Clients;
using GameProducer.Interfaces.Clients.AWS;
using GameProducer.Interfaces.Clients.Http;
using GameProducer.Interfaces.Services;
using GameProducer.Interfaces.Services.Impl;
using GameProducer.Interfaces.Strategy;
using GameProducer.Interfaces.Validators;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Npgsql;
using System;

namespace GameProducer
{
    public class Startup
    {
        private IConfiguration _config { get; }
        private readonly string ApiName;
        private readonly string ApiVersion;
        
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
            ApiName = _config.GetValue<string>("Application:Name");
            ApiVersion = _config.GetValue<string>("Application:Version");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddHealthChecks();
            services.AddNpgsqlConnectionStringBuilder(_config);
            services.AddIGDBClient(_config);

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
            
            // Hangfire
            services.AddHangfireServer();
            services.AddHangfire((provider, configuration) =>
            {
                var connectionStringBuilder = provider.GetRequiredService<NpgsqlConnectionStringBuilder>();
                configuration.UsePostgreSqlStorage(connectionStringBuilder.ToString(), new PostgreSqlStorageOptions
                {
                    SchemaName = $"hangfire-{ApiName}"
                });
                configuration.UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            });

            // strategies (singleton)
            services.AddSingleton<PublishStrategyContext>();
            services.AddSingleton<PublishStrategy, QueuePublishStrategy>();
            services.AddSingleton<PublishStrategy, TopicPublishStrategy>();
            
            // services (singleton)
            services.AddSingleton<IPublisherService, PublisherService>();
            services.AddSingleton<IGameService, GameService>();
            services.AddSingleton<IJobService, JobService>();
            services.AddSingleton<ICredentialsFacade<AwsCredentials>, AWSCredentialsFacade>();
            services.AddSingleton<ICredentialsFacade<DBCredentials>, DBCredentialsFacade>();
            services.AddSingleton<ISecretsManagerFacade, SecretsManagerFacade>();

            // validators (singleton)
            services.AddSingleton<IValidator<Game>, GameValidator>();
            services.AddSingleton<IValidator<User>, UserValidator>();

            // clients (singleton)
            services.AddSingleton<SNSClient>();
            services.AddSingleton<SQSClient>();
            services.AddSingleton<SecretsManagerClient>();
            services.AddSingleton<IGDBClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.Map($"/api/{ApiName}/v{ApiVersion}", prefixedApp =>
            {
                prefixedApp.UseRouting();
                prefixedApp.UseAuthorization();
                prefixedApp.UseEndpoints(endpoints => endpoints.MapControllers());
            });

            // Application setup
            using var scope = app.ApplicationServices.CreateScope();
            Setup(scope.ServiceProvider);
        }

        private void Setup(IServiceProvider serviceProvider)
        {
            var JobService = serviceProvider.GetService<IJobService>();
            JobService.RegisterRecurringJobs();
        }
    }
}
