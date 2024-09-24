using LibraryManagementApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;

namespace LibraryManagementApp
{
    public class LibraryDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookAllocation> BookAllocations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.HasMany(b => b.Books)
                      .WithOne(a => a.Author)
                      .HasForeignKey(k => k.AuthorId);
            });

            modelBuilder.Entity<Book>(entity =>
           {
               entity.HasKey(b => b.Id);

               entity.Property(e => e.Name)
                     .IsRequired()
                     .HasMaxLength(256);

               entity.Property(e => e.Genre)
                     .IsRequired()
                     .HasMaxLength(50);

               entity.Property(e => e.AuthorId)
                    .IsRequired();

               entity.HasMany(a => a.BookAllocations)
               .WithOne(b => b.Book)
               .HasForeignKey(k => k.BookId);
           });

            modelBuilder.Entity<User>(entity =>
         {
             entity.HasKey(u => u.Id);

             entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40);

             entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(256);

             entity.Property(e => e.AadharNo)
                    .IsRequired()
                    .HasMaxLength(16);

             entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasMaxLength(10);

             entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(40);

             entity.HasMany(a => a.BooksAllocated)
             .WithOne(b => b.User)
             .HasForeignKey(k => k.UserId);

             entity.HasIndex(a => a.AadharNo)
                   .IsUnique();

             entity.HasIndex(a => a.PhoneNo)
                  .IsUnique();

             entity.HasIndex(a => a.Email)
                  .IsUnique();
         });

            modelBuilder.Entity<BookAllocation>(entity =>
         {
             entity.HasKey(b => b.Id);

             entity.Property(e => e.ReturnDate)
                   .IsRequired();

             entity.Property(e => e.AllocationDate)
                   .IsRequired();

             entity.Property(e => e.UserId)
                  .IsRequired();

             entity.Property(e => e.BookId)
                   .IsRequired();
         });
        }
    }
}
