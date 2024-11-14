using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public interface ILoanExtensionRepository
{
	List<LoanExtension> ListLoanExtensions();
	void AddLoanExtension(LoanExtension loanExtension);
	void UpdateLoanExtension(LoanExtension loanExtension);
	void DeleteLoanExtension(int id);
}