using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebMusic.Domain.Entities;

namespace WebMusic.Infrastructure.Data
{
    public class WebMusicDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public WebMusicDbContext(DbContextOptions<WebMusicDbContext> options) : base(options)
        {
        }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<PlayHistory> PlayHistories { get; set; }
        public DbSet<Follow> Follows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ApplicationUser entity
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.Bio).HasMaxLength(500);
                entity.Property(e => e.AvatarUrl).HasMaxLength(500);
                entity.Property(e => e.Role).HasMaxLength(20).HasDefaultValue("User");
            });

            // Configure Song entity
            modelBuilder.Entity<Song>(entity =>
            {
                entity.HasKey(e => e.SongId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.FileUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.CoverImage).HasMaxLength(500);
                entity.Property(e => e.Status).HasMaxLength(50).HasDefaultValue("Pending");
                entity.Property(e => e.Duration).IsRequired();
                
                entity.HasOne<ApplicationUser>()
                    .WithMany(u => u.Songs)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Genre)
                    .WithMany(g => g.Songs)
                    .HasForeignKey(e => e.GenreId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Album)
                    .WithMany(a => a.Songs)
                    .HasForeignKey(e => e.AlbumId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Artist)
                    .WithMany(a => a.Songs)
                    .HasForeignKey(e => e.ArtistId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Album entity
            modelBuilder.Entity<Album>(entity =>
            {
                entity.HasKey(e => e.AlbumId);
                entity.Property(e => e.AlbumName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CoverImage).HasMaxLength(500);
                
                entity.HasOne<ApplicationUser>()
                    .WithMany(u => u.Albums)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Artist)
                    .WithMany(a => a.Albums)
                    .HasForeignKey(e => e.ArtistId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Playlist entity
            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.HasKey(e => e.PlaylistId);
                entity.Property(e => e.PlaylistName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                
                entity.HasOne<ApplicationUser>()
                    .WithMany(u => u.Playlists)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure PlaylistSong entity (Many-to-Many)
            modelBuilder.Entity<PlaylistSong>(entity =>
            {
                entity.HasKey(e => new { e.PlaylistId, e.SongId });
                
                entity.HasOne(e => e.Playlist)
                    .WithMany(p => p.PlaylistSongs)
                    .HasForeignKey(e => e.PlaylistId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Song)
                    .WithMany(s => s.PlaylistSongs)
                    .HasForeignKey(e => e.SongId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure Genre entity
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.GenreId);
                entity.Property(e => e.GenreName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CoverImage).HasMaxLength(500);
                entity.HasIndex(e => e.GenreName).IsUnique();
            });

            // Configure Comment entity
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.CommentId);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
                
                entity.HasOne<ApplicationUser>()
                    .WithMany(u => u.Comments)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Song)
                    .WithMany(s => s.Comments)
                    .HasForeignKey(e => e.SongId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure Like entity
            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.SongId });
                
                entity.HasOne<ApplicationUser>()
                    .WithMany(u => u.Likes)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Song)
                    .WithMany(s => s.Likes)
                    .HasForeignKey(e => e.SongId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure Artist entity
            modelBuilder.Entity<Artist>(entity =>
            {
                entity.HasKey(e => e.ArtistId);
                entity.Property(e => e.ArtistName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Biography).HasMaxLength(1000);
                entity.Property(e => e.AvatarUrl).HasMaxLength(500);
                entity.Property(e => e.CoverImage).HasMaxLength(500);
                entity.HasIndex(e => e.ArtistName).IsUnique();
            });

            // Configure PlayHistory entity
            modelBuilder.Entity<PlayHistory>(entity =>
            {
                entity.HasKey(e => e.PlayHistoryId);
                
                entity.HasOne<ApplicationUser>()
                    .WithMany(u => u.PlayHistories)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Song)
                    .WithMany(s => s.PlayHistories)
                    .HasForeignKey(e => e.SongId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure Follow entity
            modelBuilder.Entity<Follow>(entity =>
            {
                entity.HasKey(e => new { e.FollowerId, e.FollowingId });
                
                entity.HasOne<ApplicationUser>("Follower")
                    .WithMany(u => u.Following)
                    .HasForeignKey(e => e.FollowerId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne<ApplicationUser>("Following")
                    .WithMany(u => u.Followers)
                    .HasForeignKey(e => e.FollowingId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
