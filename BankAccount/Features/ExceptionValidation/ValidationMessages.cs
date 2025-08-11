namespace BankAccount.Features.ExceptionValidation
{
    public static class ValidationMessages
    {
        public static string RequiredField(string fieldName) => $"{fieldName} must not be empty.";

        public const string CurrencyTypeNotSupported = "CurrencyType is not supported.";
        public const string AccountTypeIsNotValid = "Account type is not valid.";
        public const string OpenDateCannotBeInTheFuture = "OpenDate cannot be in the future.";
        public const string OwnerIdMustReferToAnExistingOwner = "OwnerId must refer to an existing owner.";
        public const string InterestRateMustBeSetOnlyForDepositAndCreditAccounts 
            = "InterestRate must be set only for Deposit and Credit accounts.";

        public const string EmailInvalid = "Email format is invalid.";
        public const string AccountNotFound = "Account not found.";
    }
}