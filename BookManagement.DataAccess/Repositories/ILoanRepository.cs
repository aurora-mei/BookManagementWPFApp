using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public interface ILoanRepository
{
	List<Loan> ListLoan();
	void AddLoan(Loan loan);
	void UpdateLoan(Loan loan);
	void DeleteLoan(int id);
}