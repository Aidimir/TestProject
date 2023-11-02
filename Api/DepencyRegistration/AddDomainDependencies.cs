using System;
using System.Net;
using Logic.Interfaces;
using Logic.Features;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Api.MIddlewares;

namespace Api.DepencyRegistration
{
    public static class AddDomainServices
    {
        public static void AddLogicServices(this IServiceCollection services)
        {
            services
                .AddTransient<ILibraryService, LibraryService>()
                .AddTransient<GlobalExceptionHandlerMiddleware>();
                
            services.TryAddScoped<HttpClient>();
            services.TryAddScoped<WebClient>();
        }
    }
}

