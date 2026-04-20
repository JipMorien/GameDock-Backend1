using GameDock.BLL.Containers;
using Microsoft.Extensions.DependencyInjection;

namespace GameDock.BLL
{
    public static class BllServiceCollectionExtensions
    {
        public static IServiceCollection AddBllServices(this IServiceCollection services)
        {
            services.AddScoped<GameDockUserContainer>();
            services.AddScoped<ProfileContainer>();
            services.AddScoped<PostContainer>();
            services.AddScoped<LeaderboardContainer>();
            services.AddScoped<StatisticContainer>();

            return services;
        }
    }
}