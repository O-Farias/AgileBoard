using Microsoft.EntityFrameworkCore;
using AgileBoard.API.Models;

namespace AgileBoard.API.Data
{
    public class AgileBoardContext : DbContext
    {
        public AgileBoardContext(DbContextOptions<AgileBoardContext> options) : base(options) { }

        public DbSet<Board> Boards { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar relacionamentos
            modelBuilder.Entity<List>()
                .HasOne(l => l.Board)
                .WithMany()
                .HasForeignKey(l => l.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.List)
                .WithMany(l => l.Cards)
                .HasForeignKey(c => c.ListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }