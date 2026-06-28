namespace GymManagementSystemMVC.BLL.Common
{
    public sealed record Result(bool Success, string? Error = null, ResultKind Kind = ResultKind.OK)
    {
        #region Factories
        public static Result Ok() => new(true);
        public static Result Fail(string error, ResultKind kind = ResultKind.Conflict) => new(false, error, kind);
        public static Result NotFound(string error = "Not Found") => new(false, error, ResultKind.NotFound);
        public static Result Validation(string error) => new(false, error, ResultKind.ValidationFailed);
        public static Result Forbidden(string error = "Forbidden") => new(false, error, ResultKind.Forbidden);
        #endregion
    }

    public sealed record Result<T>(bool Success, T? Value, string? Error = null, ResultKind Kind = ResultKind.OK)
    {
        #region Factories
        public static Result<T> Ok(T value) => new(true, value);
        public static Result<T> Fail(string error, ResultKind kind = ResultKind.Conflict) => new(false, default, error, kind);
        public static Result<T> NotFound(string error = "Not Found") => new(false, default, error, ResultKind.NotFound);
        #endregion
    }

    public enum ResultKind
    {
        OK,
        NotFound,
        Conflict,
        ValidationFailed,
        Forbidden
    }
}
