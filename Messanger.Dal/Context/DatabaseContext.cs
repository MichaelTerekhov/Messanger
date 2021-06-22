using Messanger.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messanger.Dal.Context
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Message> Messages { get; set; }


        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Account>().HasKey(s => s.Id);

            builder.Entity<Account>().HasIndex(s => s.Id).IsUnique();

            builder.Entity<Message>().HasKey(s => s.Id);

            builder.Entity<Message>().HasIndex(s => s.Id).IsUnique();

            builder.Entity<Message>()
                .HasOne(s => s.Room)
                .WithMany(m => m.Messages)
                .HasForeignKey(s => s.Room_Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Room>().HasKey(s => s.Id);

            builder.Entity<Room>().HasIndex(s => s.Id).IsUnique();

            builder.Entity<Room>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
