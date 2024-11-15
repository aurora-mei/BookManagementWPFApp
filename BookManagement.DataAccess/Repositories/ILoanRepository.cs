using System.Linq.Expressions;
using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public interface ILoanRepository
{
	List<Loan> ListLoan();
	List<Loan> GetLoan(Expression<Func<Loan, bool>> predicate);
	void AddLoan(Loan loan);
	void UpdateLoan(Loan loan);
	void DeleteLoan(int id);
}