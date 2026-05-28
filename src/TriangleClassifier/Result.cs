namespace TriangleClassifier;

// Discriminated union: either a value or an error string. Avoids exceptions in domain logic.
public sealed class Result<T> {
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Result(T value) { IsSuccess = true; Value = value; }
    private Result(string error) { IsSuccess = false; Error = error; }

    public static Result<T> Ok(T value) => new(value);
    public static Result<T> Fail(string error) => new(error);

    // Chains operations: applies onSuccess if successful, otherwise propagates the failure.
    public Result<TNext> Map<TNext>(Func<T, Result<TNext>> onSuccess)
        => IsSuccess ? onSuccess(Value!) : Result<TNext>.Fail(Error!);
}
