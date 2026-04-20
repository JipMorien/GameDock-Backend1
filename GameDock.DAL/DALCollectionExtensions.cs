using DAL.Repos;
using DTO.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class DALCollectionExtensions
    {
        public static IServiceCollection AddDalServices(this IServiceCollection services)
        {
            services.AddScoped<IGameDockUserDAL, GameDockUserDAL>();
            services.AddScoped<IProfileDAL, ProfileDAL>();
            services.AddScoped<IPostDAL, PostDAL>();
            services.AddScoped<ILeaderboardDAL, LeaderboardDAL>();
            services.AddScoped<IStatisticDAL, StatisticDAL>();

            return services;
        }
    }
}

