using GameProducer.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Net.Http;

namespace GameProducer.Infrastructure.Extensions
{
    public static class HttpClientExtensions
    {
        public static IHttpClientBuilder AddIGDBClient(this IServiceCollection @this, IConfiguration config) 
            => @this.AddHttpClient<HttpClient>(name: "IGDBClient", (serviceProvider, options) =>
        {
            var smc = serviceProvider.GetService<ISecretsManagerFacade>();
            var IGDBSection = config.GetSection("Integration:IGDB");
            var ApiKey = GetApiKey(IGDBSection, smc);
            
            options.BaseAddress = new Uri(IGDBSection["Host"]);
            options.DefaultRequestHeaders.Add("user-key", ApiKey);
        })
        .AddTransientHttpErrorPolicy(configure => configure.WaitAndRetryAsync(retryCount: 10, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
        ));

        private static string GetApiKey(IConfigurationSection section, ISecretsManagerFacade smf)
        {
            string GetFromAppSettings()
            {
                return section["ApiKey"];
            }

            string GetFromSecretsManagerAsync()
            {
                var SecretId = section["SecretsManager"];
                return smf.GetStringProperty(SecretId);
            }

            return GetFromAppSettings() ?? GetFromSecretsManagerAsync() ?? throw new Exception("IGDB ApiKey not found");
        }
    }
}
