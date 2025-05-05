using MicrobloggingApp.Infrastructure.Interfacses;
using MicrobloggingApp.Infrastructure.Repositories;
using MicrobloggingApp.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace MicrobloggingApp.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IQueueService, QueueService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAzureBlobService, AzureBlobService>();
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtOptions>>().Value);
            services.AddSingleton<IExceptionLogger, EventViewerLogger>();

            return services;
        }
    }
}
