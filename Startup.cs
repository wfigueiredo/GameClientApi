using GameProducer.Domain.Model;
using GameProducer.Infrastructure.Extensions;
using GameProducer.Infrastructure.Security.Impl;
using GameProducer.Interfaces.Clients.Http;
using GameProducer.Interfaces.Repository;
using GameProducer.Interfaces.Repository.Impl;
using GameProducer.Interfaces.Services;
using GameProducer.Interfaces.Services.Impl;
using GameProducer.Interfaces.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PublisherApi.Interfaces.Clients.Http;
using SecretsManagerFacadeLib.Contracts;
using SecretsManagerFacadeLib.Interfaces;
using SecretsManagerFacadeLib.Interfaces.Clients;
using SecretsManagerFacadeLib.Interfaces.Clients.Impl;
using SecretsManagerFacadeLib.Interfaces.Impl;
using System;
using System.Text;

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
            services.AddNpgsqlConnectionStringBuilder(_config);
            services.AddIGDBClient(_config);
            services.AddPublisherApiClient(_config);

            var securitySection = _config.GetSection("Security");
            var encodedKey = Encoding.ASCII.GetBytes(securitySection["Token"]);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(encodedKey),
                    ValidIssuers = new string[] { securitySection["Issuer"] },
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

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
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ILoginService, LoginService>();

            // repositories
            services.AddSingleton<IUserRepository, UserRepository>();

            // infra
            services.AddSingleton<ICredentialsFacade<BasicCredentials>, BasicCredentialsFacade>();
            services.AddSingleton<ISecretsManagerFacade, SecretsManagerFacade>();
            services.AddSingleton<ICredentialsFacade<AwsCredentials>, AWSCredentialsFacade>();

            // validators (singleton)
            services.AddSingleton<IValidator<Game>, GameValidator>();
            services.AddSingleton<IValidator<User>, UserValidator>();

            // clients (singleton)
            services.AddSingleton<ISecretsManagerClient, SecretsManagerClient>(x =>
            {
                var region = "sa-east-1";
                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddConsole()
                    .AddEventLog();
                });
                var logger = loggerFactory.CreateLogger<SecretsManagerClient>();
                return new SecretsManagerClient(region, logger);
            });
            services.AddSingleton<PublisherClient>();
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
