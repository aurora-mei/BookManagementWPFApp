using BookManagement.DataAccess.Repositories;
using Util;

namespace BookManagementWPFApp.Services;

public class LoanTrackingService
{
    private readonly ILoanRepository _loanRepository;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);
    private readonly Timer _timer;

    public LoanTrackingService(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
        _timer = new Timer(TrackLoans, null, TimeSpan.Zero, _checkInterval);
    }

    private void TrackLoans(object state)
    {
        var overdueLoans = _loanRepository.GetLoan(l => l.Status.Equals(LoanStatusConstant.Borrowed) && l.DueDate <= DateTime.Now)
            .OrderBy(l => l.DueDate)
            .ToList();

        foreach (var loan in overdueLoans)
        {
            // Remove the oldest loan 
            _loanRepository.DeleteLoan(loan.LoanID);

            // Promote the next waiting loan if it exists
            var waitingLoan = _loanRepository.GetLoan(l => l.BookID == loan.BookID && l.Status.Equals(LoanStatusConstant.Waiting))
                .OrderBy(l => l.BorrowDate)
                .FirstOrDefault();
            if (waitingLoan != null)
            {
                waitingLoan.Status = LoanStatusConstant.Borrowed; // Active
                _loanRepository.UpdateLoan(waitingLoan);
            }
        }
    }

    public void Stop() => _timer?.Change(Timeout.Infinite, 0);
}
