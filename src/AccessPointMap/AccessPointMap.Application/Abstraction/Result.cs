using System;

namespace AccessPointMap.Application.Abstraction
{
    public class Result
    {
        public bool IsSuccess { get; protected init; }
        public string Message { get; protected init; }
        public bool IsFailure => !IsSuccess;

        protected internal Result(bool isSuccess, string message = null)
        {
            IsSuccess = isSuccess;

            Message = string.IsNullOrWhiteSpace(message)
                ? string.Empty : message;
        }

        public static Result Success() => new(true, null);
        public static Result Success(string message) => new(true, message);
        public static Result<TValue> Success<TValue>(TValue value) where TValue : class => new(value, true, null);

        public static Result Failure() => new(false, null);
        public static Result Failure(string message) => new(false, message);
        public static Result<TValue> Failure<TValue>(TValue value) where TValue : class => new(value, false, null);
        public static Result<TValue> Failure<TValue>(TValue value, string message) where TValue : class => new(value, false, message);
    }

    public class Result<TValue> : Result where TValue : class
    {
        private readonly TValue _value;

        protected internal Result(TValue value, bool isSuccess, string message = null) : base(isSuccess, message)
        {
            _value = value;
        }

        public TValue Value => IsFailure
            ? throw new InvalidOperationException("Can not retrieve the value from a failure result.")
            : _value;

        public static implicit operator TValue(Result<TValue> result) => result.Value;
        public static implicit operator Result<TValue>(TValue value) => new(value, true, null);
    }
}
