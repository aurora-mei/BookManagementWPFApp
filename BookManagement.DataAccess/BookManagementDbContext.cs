using BookManagement.BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

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
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }
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
            modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Order).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Book).WithMany(b => b.OrderItems).HasForeignKey(oi => oi.BookID);

            modelBuilder.Entity<Loan>().HasOne(o => o.User).WithMany(u => u.Loans).HasForeignKey(o => o.UserID);
            modelBuilder.Entity<Loan>().HasOne(o => o.Book).WithMany(b => b.Loans).HasForeignKey(o => o.BookID);
            modelBuilder.Entity<LoanExtension>().HasOne(le => le.Loan).WithMany(l => l.LoanExtensions).HasForeignKey(le => le.LoanID);

            modelBuilder.Entity<Book>().HasOne(b => b.Author).WithMany(a => a.Books).HasForeignKey(b => b.AuthorID);
            modelBuilder.Entity<Book>().HasOne(b => b.Category).WithMany(c => c.Books).HasForeignKey(b => b.CategoryID);
            modelBuilder.Entity<Book>().HasOne(b => b.Discount).WithMany(d => d.Books).HasForeignKey(b => b.DiscountID);


            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    UserID = 1,
                    Username = "admin",
                    UserStatus = UserStatusConstant.Active,
                    Password ="123",
                    Role = RoleConstant.Admin,
                    Email = "admin@admin.com"
                },
                new User()
                {
                    UserID = 2,
                    Username = "alexandra",
                    UserStatus = UserStatusConstant.Active,
                    Password ="123",
                    Role = RoleConstant.User,
                    Email = "alex@admin.com"
                });

            modelBuilder.Entity<Loan>().HasData(
                new Loan()
                {
                   LoanID = 1,
                    UserID = 2,
                    BookID = 1,
                    Status = LoanStatusConstant.Borrowed,
                    BorrowDate = new DateTime(2024, 10, 10), //nam, thang, ngay
                    DueDate = new DateTime(2024, 10, 15),
                    ReturnDate = new DateTime(2024, 10, 14),
                },
                 new Loan()
                 {
                     LoanID = 2,
                     UserID = 2,
                     BookID = 2,
                     Status = LoanStatusConstant.Borrowed,
                     BorrowDate = new DateTime(2024, 10, 10),
                     DueDate = new DateTime(2024, 10, 19),
                     ReturnDate = new DateTime(2024, 10, 11),
                 });
            var orderObj = new Order();
        modelBuilder.Entity<Order>().HasData(
               new Order()
               {
                  OrderID = 1,
                   UserID = 2,
                   ShippingMethod = "Express",
                   OrderDate = new DateTime(2024,10,15), //nam, thang, ngay
                   Status = OrderStatusConstant.Completed,
                   TotalPrice = orderObj.CalTotalPrice()
               },
                new Order()
                {
                    OrderID = 2,
                    UserID = 2,
                    ShippingMethod = "Express",
                    OrderDate = new DateTime(2024,10,29), //nam, thang, ngay
                    Status = OrderStatusConstant.Completed,
                    TotalPrice = orderObj.CalTotalPrice()
                },
                  new Order()
                  {
                      OrderID = 3,
                      UserID = 2,
                      ShippingMethod = "Express",
                      OrderDate = new DateTime(2024, 10, 29), //nam, thang, ngay
                      Status = OrderStatusConstant.Processing,
                      TotalPrice = orderObj.CalTotalPrice()
                  });

                modelBuilder.Entity<OrderItem>().HasData(
                   new OrderItem()
                   {
                     BookID = 1,
                       OrderID = 1,
                       Quantity = 2,
                     Price = 12.4,
                   },
                    new OrderItem()
                    {
                        BookID = 2,
                        OrderID = 1,
                        Quantity = 2,
                        Price = 28.4,
                    }, new OrderItem()
                    {
                        BookID = 3,
                        OrderID = 2,
                        Quantity = 1,
                        Price = 7.4,
                    },
                    new OrderItem()
                    {
                        BookID = 4,
                        OrderID = 2,
                        Quantity = 3,
                        Price = 36.4,
                    },
                     new OrderItem()
                     {
                         BookID = 3,
                         OrderID = 3,
                         Quantity = 2,
                         Price = 22.4,
                     });

            modelBuilder.Entity<Author>().HasData(
                new Author()
                {
                    AuthorID = 1,
                   AuthorName = "Landa",
                   AuthorEmail = "landa@gmail.com",
                   AuthorImageURL = "https://i.pinimg.com/736x/99/63/4f/99634f142c41939386fbabd411459929.jpg",
                },
                new Author()
                {
                    AuthorID = 2,
                    AuthorName = "Alexandra",
                    AuthorEmail = "alex@gmail.com",
                    AuthorImageURL = "https://i.pinimg.com/736x/99/63/4f/99634f142c41939386fbabd411459929.jpg"
                });
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    CategoryID = 1,
                   CategoryName = "Chidren books",
                },
                new Category()
                {
                    CategoryID = 2,
                    CategoryName = "Philosophy",
                });
            
      
            modelBuilder.Entity<Book>().HasData(
                new Book()
                {
                    BookID = 3,
                   BookImages = "https://i.pinimg.com/736x/99/63/4f/99634f142c41939386fbabd411459929.jpg",
                   Title = "Doraemon",
                   Price = 12.4,
                   Pages = 123,
                   PublishDate = new DateTime(2024, 12, 10), //nam, thang, ngay
                   CategoryID = 1,
                   AuthorID = 1,
                   Language = "English",
                   Description = "Winner of the 2024 Hawthornden Prize\nShortlisted for the 2024 Orwell Prize for Political Fiction\nShortlisted for the 2024 Ursula K. Le Guin Prize for Fiction\n\nA singular new novel from Betty Trask Prize-winner Samantha Harvey, Orbital is an eloquent meditation on space and life on our planet through the eyes of six astronauts circling the earth in 24 hours\n\n\"Ravishingly beautiful.\" — Joshua Ferris, New York Times\n\nA slender novel of epic power and the winner of the Booker Prize 2024, Orbital deftly snapshots one day in the lives of six women and men traveling through space. Selected for one of the last space station missions of its kind before the program is dismantled, these astronauts and cosmonauts—from America, Russia, Italy, Britain, and Japan—have left their lives behind to travel at a speed of over seventeen thousand miles an hour as the earth reels below. We glimpse moments of their earthly lives through brief communications with family, their photos and talismans; we watch them whip up dehydrated meals, float in gravity-free sleep, and exercise in regimented routines to prevent atrophying muscles; we witness them form bonds that will stand between them and utter solitude. Most of all, we are with them as they behold and record their silent blue planet. Their experiences of sixteen sunrises and sunsets and the bright, blinking constellations of the galaxy are at once breathtakingly awesome and surprisingly intimate.\n\nProfound and contemplative, Orbital is a moving elegy to our environment and planet."
                },
                new Book()
                {
                    BookID = 4,
                   BookImages = "https://i.pinimg.com/736x/99/63/4f/99634f142c41939386fbabd411459929.jpg",
                   Title = "Pikachu",
                   Price = 12.4,
                   Pages = 123,
                   PublishDate = new DateTime(2024, 10, 23), //nam, thang, ngay
                   CategoryID = 1,
                   AuthorID = 1,
                   Language = "English",
                   Description = "Alone in space, years from rescue, everyone she knows has vanished.\nOn a colonial mission into uncharted space, Dr. Beth Adler awakens to find her ship ravaged and abandoned. The last thing she recalls is an alarm repeating the same horrifying message. “Quarantine breach.”"
                 }, new Book()
                 {
                     BookID = 1,
                     BookImages = "https://i.pinimg.com/736x/99/63/4f/99634f142c41939386fbabd411459929.jpg",
                     Title = "7 vien ngoc rong",
                     Price = 12.4,
                     Pages = 123,
                     PublishDate = new DateTime(2024,10,12),//nam, thang, ngay
                     CategoryID = 1,
                     AuthorID = 1,
                     Language = "English",
                     Description = "Winner of the 2024 Hawthornden Prize\nShortlisted for the 2024 Orwell Prize for Political Fiction\nShortlisted for the 2024 Ursula K. Le Guin Prize for Fiction\n\nA singular new novel from Betty Trask Prize-winner Samantha Harvey, Orbital is an eloquent meditation on space and life on our planet through the eyes of six astronauts circling the earth in 24 hours\n\n\"Ravishingly beautiful.\" — Joshua Ferris, New York Times\n\nA slender novel of epic power and the winner of the Booker Prize 2024, Orbital deftly snapshots one day in the lives of six women and men traveling through space. Selected for one of the last space station missions of its kind before the program is dismantled, these astronauts and cosmonauts—from America, Russia, Italy, Britain, and Japan—have left their lives behind to travel at a speed of over seventeen thousand miles an hour as the earth reels below. We glimpse moments of their earthly lives through brief communications with family, their photos and talismans; we watch them whip up dehydrated meals, float in gravity-free sleep, and exercise in regimented routines to prevent atrophying muscles; we witness them form bonds that will stand between them and utter solitude. Most of all, we are with them as they behold and record their silent blue planet. Their experiences of sixteen sunrises and sunsets and the bright, blinking constellations of the galaxy are at once breathtakingly awesome and surprisingly intimate.\n\nProfound and contemplative, Orbital is a moving elegy to our environment and planet."
                 },
                new Book()
                {
                    BookID = 2,
                    BookImages = "https://i.pinimg.com/736x/99/63/4f/99634f142c41939386fbabd411459929.jpg",
                    Title = "Batman",
                    Price = 12.4,
                    Pages = 123,
                    PublishDate = new DateTime(2024, 10, 23), //nam, thang, ngay
                    CategoryID = 1,
                    AuthorID = 1,
                    Language = "English",
                    Description = "Alone in space, years from rescue, everyone she knows has vanished.\nOn a colonial mission into uncharted space, Dr. Beth Adler awakens to find her ship ravaged and abandoned. The last thing she recalls is an alarm repeating the same horrifying message. “Quarantine breach.”"
                });
        }
    }
}
