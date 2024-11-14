using System.Linq.Expressions;
using BookManagement.BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.DataAccess.Repositories;

public class LoanRepository : ILoanRepository
{
	public List<Loan> ListLoan()
	{
		var db = new BookManagementDbContext();
		return db.Loans.Include(l => l.Book).Include(l => l.User).ToList();
	}

	public List<Loan> GetLoan(Expression<Func<Loan, bool>> predicate)
	{
		var db = new BookManagementDbContext();
		return db.Loans.Where(predicate).Include(l => l.Book).Include(l => l.User).ToList();
	}

	public void AddLoan(Loan loan)
	{
		var db = new BookManagementDbContext();
		db.Loans.Add(loan);
	}

	public void UpdateLoan(Loan loan)
	{
		var db = new BookManagementDbContext();
		var existingLoan = db.Loans.FirstOrDefault(x => x.LoanID.Equals(loan.LoanID));
		if (existingLoan != null)
		{
			existingLoan.BookID = loan.BookID;
			existingLoan.UserID = loan.UserID;
			//existingLoan.LoanID = loan.LoanID;
			existingLoan.Status = loan.Status;
			existingLoan.BorrowDate = loan.BorrowDate;
			existingLoan.ReturnDate = loan.ReturnDate;
			existingLoan.DueDate = loan.DueDate;
			existingLoan.FineAmount = loan.FineAmount;
			
			db.SaveChanges();
		}
		else
		{
			throw new Exception("Loan not found");
		}
	}

	public void DeleteLoan(int id)
	{
		var db = new BookManagementDbContext();
		var loan = db.Loans.FirstOrDefault(x => x.LoanID.Equals(id));
		if (loan != null)
		{
			db.Loans.Remove(loan);
			db.SaveChanges();
		}
		else
		{
			throw new Exception("Loan not found");
		}
	}
}