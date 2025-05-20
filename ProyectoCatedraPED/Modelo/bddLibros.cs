using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ProyectoCatedraPED.Modelo
{
    public partial class bddLibros : DbContext
    {
        public bddLibros()
            : base("name=bibliotech")
        {
        }

        public virtual DbSet<BookRating> BookRatings { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Score> Scores { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.autor)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.image_name)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.BookRatings)
                .WithRequired(e => e.Book)
                .HasForeignKey(e => e.book_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Scores)
                .WithRequired(e => e.Book)
                .HasForeignKey(e => e.book_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Genre>()
                .Property(e => e.genre_name)
                .IsUnicode(false);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.Genre)
                .HasForeignKey(e => e.genre_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Scores)
                .WithRequired(e => e.Genre)
                .HasForeignKey(e => e.genre_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Score>()
                .Property(e => e.score1)
                .HasPrecision(8, 2);

            modelBuilder.Entity<User>()
                .Property(e => e.fullname)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.BookRatings)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);
        }
    }
}
