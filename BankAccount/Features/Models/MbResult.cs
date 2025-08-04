namespace BankAccount.Features.Models
{
    public class MbResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }
        public Dictionary<string, string>? ValidationErrors { get; init; }


        public static MbResult<T> Ok(T data) => new() { Success = true, Data = data };
        public static MbResult<T> Fail(string error) => new() { Success = false, Error = error };
        public static MbResult<T> Fail(string error, Dictionary<string, string> validationErrors) =>
            new() { Success = false, Error = error, ValidationErrors = validationErrors };

    }
}