using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibApp.Models;
using WebLibApp.ViewModels;

namespace WebLibApp.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    // public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
                    : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowHistory> BorrowHistories { get; set; }
        public DbSet<Copy> Copies { get; set; }
        public DbSet<BookReservation> BookReservations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            /* modelBuilder
            .Entity<Book>().
            HasOne(c => c.Author).WithMany(i => i.Books).HasForeignKey(c => c.Author);*/

            modelBuilder
            .Entity<Copy>()
              .HasOne(c => c.Book).WithMany(i => i.Copies).HasForeignKey(c => c.BookId);
            
            modelBuilder
           .Entity<BookReservation>()
           .HasOne(c => c.Copy).WithMany(i => i.BookReservations).HasForeignKey(c => c.CopyId);

            modelBuilder
            .Entity<BookReservation>()
            .HasOne(c => c.User).WithMany(i => i.BookReservations).HasForeignKey(c => c.AppUserId);

            modelBuilder
           .Entity<BorrowHistory>()
           .HasOne(c => c.Copy).WithMany(i => i.BorrowHistories).HasForeignKey(c => c.CopyId);

            modelBuilder
            .Entity<BorrowHistory>()
            .HasOne(c => c.User).WithMany(i => i.BorrowHistories).HasForeignKey(c => c.AppUserId);
        }

    }
}

