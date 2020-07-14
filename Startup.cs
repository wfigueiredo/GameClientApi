using Amazon;
using GameClientApi.Domain.Model;
using GameClientApi.Infrastructure.Extensions;
using GameClientApi.Interfaces.Clients.Http;
using GameClientApi.Interfaces.Services;
using GameClientApi.Interfaces.Services.Impl;
using GameClientApi.Interfaces.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PublisherApi.Interfaces.Clients.Http;
using SecretsManagerFacadeLib.Contracts;
using SecretsManagerFacadeLib.Interfaces;
using SecretsManagerFacadeLib.Interfaces.Clients;
using SecretsManagerFacadeLib.Interfaces.Clients.Impl;
using SecretsManagerFacadeLib.Interfaces.Impl;

namespace GameClientApi
{
    public class Startup
    {
        private IConfiguration _config { get; }
        private readonly string ApiName;
        private readonly string ApiVersion;
        
        public Startup(IConfiguration configuration)
        {
            _config = configuration;

            var ApiSection = _config.GetSection("Application");
            ApiName = ApiSection["Name"];
            ApiVersion = ApiSection["Version"];
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddHealthChecks();
            services.AddHttpContextAccessor();

            // extensions
            services.AddIGDBClient(_config);
            services.AddPublisherApiClient(_config);

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
            
            // services (singleton)
            services.AddSingleton<IGameService, GameService>();

            // infra
            services.AddSingleton<ISecretsManagerFacade, SecretsManagerFacade>();
            services.AddSingleton<ICredentialsFacade<AwsCredentials>, AWSCredentialsFacade>();

            // validators (singleton)
            services.AddSingleton<IValidator<Game>, GameValidator>();

            // clients (singleton)
            services.AddSingleton(RegionEndpoint.SAEast1);
            services.AddSingleton<ISecretsManagerClient, SecretsManagerClient>();
            services.AddSingleton<PublisherClient>();
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // global cors policy
            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // SwaggerUI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{ApiName} v{ApiVersion}");
                c.RoutePrefix += $"/api/{ApiName}/v{ApiVersion}";
            });
            app.UseReDoc(c =>
            {
                c.SpecUrl = "/Swagger/webhook.yaml";
                c.DocumentTitle = $"{ApiName} v{ApiVersion}";
                c.RoutePrefix = "webhook-docs";
            });
        }
    }
}
