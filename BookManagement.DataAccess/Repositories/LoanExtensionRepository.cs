using BookManagement.BusinessObjects;

namespace BookManagement.DataAccess.Repositories;

public class LoanExtensionRepository : ILoanExtensionRepository
{
	public List<LoanExtension> ListLoanExtensions()
	{
		var db = new BookManagementDbContext();
		return db.LoanExtensions.ToList();
	}

	public void AddLoanExtension(LoanExtension loanExtension)
	{
		var db = new BookManagementDbContext();
		db.LoanExtensions.Add(loanExtension);
	}

	public void UpdateLoanExtension(LoanExtension loanExtension)
	{
		var db = new BookManagementDbContext();
		var existingLoanExtension = db.LoanExtensions.FirstOrDefault(x => x.LoanItemID.Equals(loanExtension.LoanItemID));
		if (existingLoanExtension != null)
		{
			//existingLoanExtension.LoanItemID = loanExtension.LoanItemID;
			existingLoanExtension.LoanID = loanExtension.LoanID;
			existingLoanExtension.ExtensionDate = loanExtension.ExtensionDate;
			existingLoanExtension.ExtendedDueDate = loanExtension.ExtendedDueDate;
			
			db.SaveChanges();
		}
		else
		{
			throw new Exception("LoanExtension not found");
		}
	}

	public void DeleteLoanExtension(int id)
	{
		var db = new BookManagementDbContext();
		var loanExtension = db.LoanExtensions.FirstOrDefault(x => x.LoanItemID.Equals(id));
		if (loanExtension != null)
		{
			db.LoanExtensions.Remove(loanExtension);
			db.SaveChanges();
		}
		else
		{
			throw new Exception("LoanExtension not found");
		}
	}
}