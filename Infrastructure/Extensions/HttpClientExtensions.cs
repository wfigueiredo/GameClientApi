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
            var IGDBSection = config.GetSection("Integration:IGDB");
            var smc = serviceProvider.GetService<SecretsManagerFacade>();
            
            options.BaseAddress = new Uri(IGDBSection["Host"]);
            options.DefaultRequestHeaders.Add("user-key", GetIGDBApiKey(smc));
        })
        .AddTransientHttpErrorPolicy(configure => configure.WaitAndRetryAsync(retryCount: 10, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
        ));

        private static string GetIGDBApiKey(SecretsManagerFacade smf)
        {
            return smf.GetStringProperty(SecretsManagerFacade.SECRET_NAME_IGDB_API_KEY);
        }
    }
}
