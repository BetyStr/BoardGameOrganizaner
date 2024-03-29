using async_bgg.model.business;
using Microsoft.EntityFrameworkCore;

namespace async_bgg.DAL
{
    public sealed class BggDbContext : DbContext
    {
        public DbSet<Game> Games { get; init; }
        public DbSet<Player> Players { get; init; }
        public DbSet<Session> Sessions { get; init; }

        public DbSet<SessionPlayer> SessionPlayers { get; set; }

        public BggDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=BggDB.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SessionPlayer>()
                .HasKey(p => new {p.PlayerId, p.SessionId});

            modelBuilder.Entity<SessionPlayer>()
                .HasOne<Session>(s => s.Session)
                .WithMany(s => s.SessionPlayers)
                .HasForeignKey(c => c.SessionId);


            modelBuilder.Entity<SessionPlayer>()
                .HasOne<Player>(p => p.Player)
                .WithMany(p => p.ParticipatedSessions)
                .HasForeignKey(c => c.PlayerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}