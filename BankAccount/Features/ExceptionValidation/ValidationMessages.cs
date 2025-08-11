namespace BankAccount.Features.ExceptionValidation
{
    public static class ValidationMessages
    {
        public static string RequiredField(string fieldName) => $"{fieldName} is required.";
        public static string MustExistsField(string fieldName) => $"{fieldName} must exist.";

        public const string CurrencyTypeNotSupported = "CurrencyType is not supported.";
        public const string AccountTypeIsNotValid = "Account type is not valid.";
        public const string OpenDateCannotBeInTheFuture = "OpenDate cannot be in the future.";
        public const string OwnerIdMustReferToAnExistingOwner = "OwnerId must refer to an existing owner.";
        public const string InterestRateMustBeSetOnlyForDepositAndCreditAccounts 
            = "InterestRate must be set only for Deposit and Credit accounts.";

        public const string TimestampCannotBeInTheFuture = "Timestamp cannot be in the future.";

        public const string TheDataIsOutdate = "The data is outdated, repeat the operation.";

        public const string EmailInvalid = "Email format is invalid.";
        public const string AccountNotFound = "Account not found.";
    }
}