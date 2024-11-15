using BookManagement.BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Util;

namespace BookManagement.DataAccess.Repositories;

public class BookRepository : IBookRepository
{
	public List<Book> GetListBooks()
	{
		using  var db = new BookManagementDbContext();
		return db.Books.Include(x => x.Author).Include(x => x.Category)
			.Include(x => x.OrderItems).Include(x => x.Loans).ToList();
	}
	public Book? GetBookById(int id)
	{
		using  var db = new BookManagementDbContext();
		return db.Books.Include(x => x.Author).Include(x => x.Category)
			.Include(x => x.OrderItems).Include(x => x.Loans).FirstOrDefault(x => x.BookID == id);
	}

	public void AddBook(Book book)
	{
		using  var db = new BookManagementDbContext();
		db.Books.Add(book);
		db.SaveChanges();
	}

	public void UpdateBook(Book book)
	{
		using  var db = new BookManagementDbContext();
		var bookToUpdate = db.Books.FirstOrDefault(b => b.BookID.Equals(book.BookID));
		if (bookToUpdate != null)
		{
			bookToUpdate.Title = book.Title;
			bookToUpdate.AuthorID = book.AuthorID;
			bookToUpdate.Price = book.Price;
			bookToUpdate.Quantity = book.Quantity;
			bookToUpdate.PublishDate = book.PublishDate;
			bookToUpdate.Description = book.Description;
			bookToUpdate.CategoryID = book.CategoryID;
			bookToUpdate.Language = book.Language;
			bookToUpdate.DiscountID = book.DiscountID;
			bookToUpdate.BookImages = book.BookImages;
			db.Books.Update(bookToUpdate);
			db.SaveChanges();
		}
		else
		{
			throw new Exception("Book not found");
		}
	}

	public void DeleteBook(int id)
	{
		using  var db = new BookManagementDbContext();
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

    public List<Book> GetBorrowedBooksOfUser(int userId)
    {
        using  var db = new BookManagementDbContext();
		var loanOfUser = db.Loans.Include(x => x.Book).Where(x => x.UserID == userId);
        var borrowedBookIds = loanOfUser
		.Where(x => x.Status == LoanStatusConstant.Borrowed)
		.Select(x => x.BookID)
		.ToList();

        var borrowedBooks = db.Books.Include(x => x.Author).Include(x => x.Category)
            .Include(x => x.OrderItems).Include(x => x.Loans)
            .Where(book => borrowedBookIds.Contains(book.BookID))
            .ToList();
		return borrowedBooks;
    }
}