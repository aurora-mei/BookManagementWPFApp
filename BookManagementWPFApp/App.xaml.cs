using System;
using System.Configuration;
using System.Data;
using System.Windows;
using BookManagementWPFApp.Services;
using BookManagement.DataAccess.Repositories;

namespace BookManagementWPFApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private LoanTrackingService _loanTrackingService;
        //start loan timer when the app start
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize the LoanRepository
            ILoanRepository loanRepository = new LoanRepository(); // Ensure correct dependency setup

            // Initialize and start the LoanTrackingService
            _loanTrackingService = new LoanTrackingService(loanRepository);

            // Optional: Log service start-up or handle any additional setup
            Console.WriteLine("Loan tracking service started.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Ensure the service stops when the application exits
            _loanTrackingService?.Stop();
            Console.WriteLine("Loan tracking service stopped.");

            base.OnExit(e);
        }
    }

}
