using Microsoft.EntityFrameworkCore;

namespace spilchat_api.Models
{
    public class SpilchatDbContext : DbContext
    {
        public SpilchatDbContext(DbContextOptions<SpilchatDbContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }  // UserController için gerekli

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(e =>
            {
                e.ToTable("Messages");

                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();

                e.Property(x => x.FromUser)
                    .HasMaxLength(50)
                    .IsRequired();

                e.Property(x => x.ToUser)
                    .HasMaxLength(50)
                    .IsRequired();

                // Sınıftaki Text -> DB'deki Message
                e.Property(x => x.Text)
                    .HasColumnName("Message")
                    .IsRequired();

                // Timestamp'i DB üretsin (datetime2 + UTC default)
                e.Property(x => x.Timestamp)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("SYSUTCDATETIME()")
                    .ValueGeneratedOnAdd();

                e.Property(x => x.IsRead)
                    .HasDefaultValue(false);

                // Sorgu performansı için iki yönlü indeksler
                e.HasIndex(x => new { x.FromUser, x.ToUser, x.Timestamp })
                    .HasDatabaseName("IX_Messages_From_To_Time");

                e.HasIndex(x => new { x.ToUser, x.FromUser, x.Timestamp })
                    .HasDatabaseName("IX_Messages_To_From_Time");
            });

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();

                // İstersen:
                // e.HasIndex(u => u.Username).IsUnique();
            });
        }
    }
}
