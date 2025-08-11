using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BankAccount.Features.Accounts.Create;
using BankAccount.Features.Models.DTOs;
using BankAccount.Features.Models.Enums;

namespace BankAccount.BankAccount.UnitTests
{
    public class ParallelTransferTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ParallelTransferTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Test_AccountCreateEndpoint_IsAvailable()
        {
            var response = await _client.GetAsync("/swagger/v1/swagger.json");
            Assert.True(response.IsSuccessStatusCode);

            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/Account/create", content);

            await response.Content.ReadAsStringAsync();
            Assert.NotEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotEqual(System.Net.HttpStatusCode.NotFound, postResponse.StatusCode);
        }

        [Fact]
        public async Task ParallelTransfers_ShouldPreserveTotalBalance()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "test-token");

            var lastBalance = await _client.GetAsync("Test/totalbalance");
            lastBalance.EnsureSuccessStatusCode();

            var lastBalanceString = await lastBalance.Content.ReadAsStringAsync();

            var lastBalanceValue = decimal.Parse(lastBalanceString, System.Globalization.CultureInfo.InvariantCulture);

            await CreateAccountIfNotExists("11111111-1111-1111-1111-111111111111");
            await CreateAccountIfNotExists("22222222-2222-2222-2222-222222222222");

            const int parallelTransfersCount = 50;
            var tasks = new Task[parallelTransfersCount];

            for (int i = 0; i < parallelTransfersCount; i++)
            {
                tasks[i] = TransferAsync();
            }

            await Task.WhenAll(tasks);

            var balanceResponse = await _client.GetAsync("Test/totalbalance");
            balanceResponse.EnsureSuccessStatusCode();

            var balanceString = await balanceResponse.Content.ReadAsStringAsync();

            var totalBalance = decimal.Parse(balanceString, System.Globalization.CultureInfo.InvariantCulture);

            Assert.Equal(lastBalanceValue, totalBalance);
        }

        private async Task TransferAsync()
        {
            TransferDto transferData = new TransferDto
            {
                From = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                To = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Amount = 100m,
                Description = "123123123"
            };

            var content = new StringContent(JsonSerializer.Serialize(transferData), Encoding.UTF8, "application/json");
            await _client.PostAsync("/transaction/transfer", content);
        }

        private async Task CreateAccountIfNotExists(string accountId)
        {
            var getResponse = await _client.GetAsync($"{accountId}/exists");
            if (getResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                AccountDto newAccount = new AccountDto
                {
                    Id = new Guid(accountId),
                    OwnerId = new Guid(accountId),
                    Type = AccountType.Credit,
                    Currency = CurrencyType.Rub,
                    Balance = 1000,
                    OpenDate = DateTime.Parse("2025-01-1T13:32:15.111Z").ToUniversalTime(),
                    InterestRate = 0
                };

                var createAccountCommand = new CreateAccountCommand(newAccount);

                var content = new StringContent(
                    JsonSerializer.Serialize(createAccountCommand), Encoding.UTF8, "application/json");

                await _client.PostAsync("http://localhost:5000/Account/create", content);
            }
        }
    }
}