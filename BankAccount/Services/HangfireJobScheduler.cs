using BankAccount.Services.Interfaces;
using Hangfire;

namespace BankAccount.Services
{
    public class HangfireJobScheduler : IHangfireJobScheduler
    {
        private readonly IRecurringJobManager _recurringJobManager;

        public HangfireJobScheduler(IRecurringJobManager recurringJobManager)
        {
            _recurringJobManager = recurringJobManager;
        }

        public void ScheduleJobs()
        {
            _recurringJobManager.AddOrUpdate<AccountService>(
                "accrue-interest-job",
                service => service.AccrueInterestForAllAccountsAsync(),
                Cron.Daily);
        }
    }
}