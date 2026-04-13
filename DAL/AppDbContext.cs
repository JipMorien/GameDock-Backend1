using Domain;
using Domain.Statistics;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<GameDockUser> GameDockUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }
    }
}