namespace BankAccount.Services.Interfaces
{
    public interface IHangfireJobScheduler
    {
        void ScheduleJobs();
    }
}