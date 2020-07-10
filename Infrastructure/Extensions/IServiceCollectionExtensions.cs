using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SecretsManagerFacadeLib.Contracts;
using SecretsManagerFacadeLib.Interfaces;
using System;

namespace GameProducer.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddNpgsqlConnectionStringBuilder(this IServiceCollection @this, IConfiguration configuration) 
            => @this.AddSingleton(provider =>
        {
            var postgresConfig = configuration.GetSection("PostgreSQL");
            
            if (postgresConfig == null) 
                throw new Exception("Cannot find database configuration section in appsettings.json");

            var credentialsFacade = provider.GetService<ICredentialsFacade<BasicCredentials>>();
            return BuildPgsqlConnectionString(postgresConfig, credentialsFacade);
        });

        private static NpgsqlConnectionStringBuilder BuildPgsqlConnectionString(IConfigurationSection postgresConfig, ICredentialsFacade<BasicCredentials> credentialsFacade)
        {
            var credentials = credentialsFacade.GetCredentials();

            return new NpgsqlConnectionStringBuilder
            {
                Database = postgresConfig["database"],
                Host = postgresConfig["host"],
                Username = credentials.Username,
                Password = credentials.Password,
                MinPoolSize = Convert.ToInt32(postgresConfig["minPoolSize"]),
                MaxPoolSize = Convert.ToInt32(postgresConfig["maxPoolSize"]),
            };
        }
    }
}