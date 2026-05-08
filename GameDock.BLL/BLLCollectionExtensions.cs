using GameDock.BLL.Containers;
using GameDock.BLL.Services;
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
            services.AddScoped<AuthContainer>();
            services.AddScoped<JwtTokenService>();
            services.AddScoped<PasswordService>();
            services.AddScoped<FriendRequestContainer>();

            return services;
        }
    }
}