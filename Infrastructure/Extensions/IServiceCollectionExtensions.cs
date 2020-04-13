using GameProducer.Infrastructure.Contracts;
using GameProducer.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
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

            var smd = provider.GetService<SecretsManagerFacade>();
            return BuildPgsqlConnectionString(postgresConfig, smd);
        });

        private static NpgsqlConnectionStringBuilder BuildPgsqlConnectionString(IConfigurationSection postgresConfig, SecretsManagerFacade smc)
        {
            GenericCredentials GetSecretFromSecretsManagerAsync() =>
                smc.GetObjectProperty<GenericCredentials>(SecretsManagerFacade.SECRET_NAME_RDS_CREDENTIALS);

            GenericCredentials GetSecretFromAppsettings()
            {
                return new GenericCredentials 
                { 
                    Username = postgresConfig["username"], 
                    Password = postgresConfig["password"] 
                };
            }

            var credentials = 
                GetSecretFromAppsettings() ?? 
                GetSecretFromSecretsManagerAsync() ?? 
                    throw new Exception("Could not retrieve connection string for PostgreSQL");

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