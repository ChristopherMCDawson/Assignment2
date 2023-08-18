using Assignment2.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Data
{
    public class SportsDbContext : DbContext
    {
        public SportsDbContext(DbContextOptions<SportsDbContext> options) : base(options)
        {

        }

        public DbSet<Fan> Fans { get; set; }
        public DbSet<SportClub> SportClubs { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<News> News { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>().HasKey(s => new { s.FanId, s.SportClubId });
            modelBuilder.Entity<News>().ToTable("news");
            modelBuilder.Entity<Fan>().ToTable("Fan");
            modelBuilder.Entity<SportClub>().ToTable("SportClub");
            modelBuilder.Entity<Subscription>().ToTable("Subscription");

        }

    }
}