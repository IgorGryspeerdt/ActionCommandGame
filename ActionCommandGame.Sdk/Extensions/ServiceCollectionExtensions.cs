using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using ActionCommandGame.Sdk.Handlers;
using System;

namespace ActionCommandGame.Sdk.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddActionCommandGameSdk(
            this IServiceCollection services,
            string baseUrl,
            Func<string?> getToken)
        {
            services.AddTransient<AuthorizationHandler>(_ => new AuthorizationHandler(getToken));

            services.AddHttpClient<AuthSdkService>(c => c.BaseAddress = new Uri(baseUrl));
            services.AddHttpClient<PlayerSdkService>(c => c.BaseAddress = new Uri(baseUrl))
                .AddHttpMessageHandler<AuthorizationHandler>();
            services.AddHttpClient<ItemSdkService>(c => c.BaseAddress = new Uri(baseUrl))
                .AddHttpMessageHandler<AuthorizationHandler>();
            services.AddHttpClient<GameSdkService>(c => c.BaseAddress = new Uri(baseUrl))
                .AddHttpMessageHandler<AuthorizationHandler>();

            return services;
        }
    }
}
