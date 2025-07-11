using System.Diagnostics.CodeAnalysis;

namespace Recepify.Core.ResultPattern;

public class Result<T> : IResultBase
{

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = null;
    }

/* <<<<<<<<<<<<<<  ✨ Windsurf Command ⭐ >>>>>>>>>>>>>>>> */
/// <summary>
/// Initializes a new instance of the <see cref="Result{T}"/> class with an error.
/// </summary>
/// <param name="error">The error that represents the failure result.</param>

/* <<<<<<<<<<  af8835b7-3eca-4cf7-ab31-d9c61b5a12f2  >>>>>>>>>>> */
    private Result(Error error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; set; }
    public T? Value { get; set; }
    public Error? Error { get; set; }


    public static Result<T> Success(T value) => new(value);
    public static Result<T> Fail(Error error) => new(error);

    public object? GetValue() => Value;

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Fail(error);
}

public sealed record Error(int StatusCode, string Title, string Description)
{
    public static readonly Error None = new Error(500, string.Empty, string.Empty);
}

public interface IResultBase
{
    bool IsSuccess { get; }
    Error? Error { get; }
    object? GetValue(); // Method to get the value without knowing T
}
