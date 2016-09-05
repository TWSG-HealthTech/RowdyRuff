using Microsoft.EntityFrameworkCore;
using RowdyRuff.Core.Common;

namespace RowdyRuff.Repository
{
    public class RowdyRuffContext : DbContext
    {
        public RowdyRuffContext(DbContextOptions<RowdyRuffContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientProfile>(builder =>
            {
                builder.HasKey(p => p.Id);
                builder.HasMany(p => p.Subscriptions)
                       .WithOne()
                       .HasForeignKey(s => s.ClientProfileId);
                builder.HasMany(p => p.Connections)
                    .WithOne()
                    .HasForeignKey(c => c.ClientProfileId);
            });

            modelBuilder.Entity<SocialConnection>(builder =>
            {
                builder.HasKey(s => s.Id);
                builder.Property(s => s.Id)
                    .ValueGeneratedOnAdd();
                builder.Ignore(s => s.Aliases);
            });

            modelBuilder.Entity<Subscription>(builder =>
            {
                builder.HasKey(s => s.Id);
                builder.Property(s => s.Id)
                    .ValueGeneratedOnAdd();
            });
        }
    }
}
