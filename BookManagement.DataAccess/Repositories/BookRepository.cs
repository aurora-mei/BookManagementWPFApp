using BookManagement.BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.DataAccess.Repositories;

public class BookRepository : IBookRepository
{
    public List<Book> GetListBooks()
    {
        var db = new BookManagementDbContext();
        return db.Books.Include(b => b.Discount).Include(b => b.Category).Include(b => b.Author).ToList();
    }

    public void AddBook(Book book)
    {
        var db = new BookManagementDbContext();
        db.Books.Add(book);
    }

    public void UpdateBook(Book book)
    {
        var db = new BookManagementDbContext();
        var bookToUpdate = db.Books.FirstOrDefault(b => b.BookID.Equals(book.BookID));
        if (bookToUpdate != null)
        {
            bookToUpdate.Title = book.Title;
            bookToUpdate.AuthorID = book.AuthorID;
            bookToUpdate.Price = book.Price;
            bookToUpdate.Pages = book.Pages;
            bookToUpdate.Quantity = book.Quantity;
            bookToUpdate.PublishDate = book.PublishDate;
            bookToUpdate.Description = book.Description;
            bookToUpdate.CategoryID = book.CategoryID;
            bookToUpdate.Language = book.Language;
            bookToUpdate.DiscountID = book.DiscountID;
            bookToUpdate.BookPDFLink = book.BookPDFLink;
            bookToUpdate.BookImages = book.BookImages;
            bookToUpdate.VisitedNumber = book.VisitedNumber;
            db.SaveChanges();
        }
        else
        {
            throw new Exception("Book not found");
        }
    }

    public void DeleteBook(int id)
    {
        var db = new BookManagementDbContext();
        var bookToDelete = db.Books.FirstOrDefault(b => b.BookID.Equals(id));
        if (bookToDelete != null)
        {
            db.Books.Remove(bookToDelete);
            db.SaveChanges();
        }
        else
        {
            throw new Exception("Book not found");
        }
    }
}