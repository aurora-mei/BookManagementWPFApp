using BookManagement.BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.DataAccess
{
    public class BookManagementDbContext:DbContext
    {
        public BookManagementDbContext()
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanExtension> LoanExtensions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("BookManagementDB"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasKey(b => b.BookID);
            modelBuilder.Entity<Author>().HasKey(a => a.AuthorID);
            modelBuilder.Entity<Category>().HasKey(c => c.CategoryID);
            modelBuilder.Entity<Discount>().HasKey(d => d.DiscountID);
            modelBuilder.Entity<Loan>().HasKey(l => l.LoanID);
            modelBuilder.Entity<LoanExtension>().HasKey(le => le.LoanItemID);
            modelBuilder.Entity<Order>().HasKey(d => d.OrderID);
            modelBuilder.Entity<OrderItem>().HasKey(d => new { d.OrderID,d.BookID});
            modelBuilder.Entity<User>().HasKey(d => d.UserID);

            modelBuilder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserID);
            modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Order).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderID);
            modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Book).WithMany(b => b.OrderItems).HasForeignKey(oi => oi.BookID);

            modelBuilder.Entity<Loan>().HasOne(o => o.User).WithMany(u => u.Loans).HasForeignKey(o => o.UserID);
            modelBuilder.Entity<Loan>().HasOne(o => o.Book).WithMany(b => b.Loans).HasForeignKey(o => o.BookID);
            modelBuilder.Entity<LoanExtension>().HasOne(le => le.Loan).WithMany(l => l.LoanExtensions).HasForeignKey(le => le.LoanID);

            modelBuilder.Entity<Book>().HasOne(b => b.Author).WithMany(a => a.Books).HasForeignKey(b => b.AuthorID);
            modelBuilder.Entity<Book>().HasOne(b => b.Category).WithMany(c => c.Books).HasForeignKey(b => b.CategoryID);
            modelBuilder.Entity<Book>().HasOne(b => b.Discount).WithMany(d => d.Books).HasForeignKey(b => b.DiscountID);
        }
    }
}
