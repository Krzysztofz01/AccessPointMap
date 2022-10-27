using System;

namespace AccessPointMap.Application.Core
{
    public class Result
    {
        public bool IsSuccess { get; protected init; }
        public bool IsFailure => !IsSuccess;

        protected readonly string _message;
        public string Message => IsFailure
            ? throw new InvalidOperationException("Can not retrieve the message from a failure result.")
            : _message;

        protected readonly Error _error;
        public Error Error => IsSuccess
            ? throw new InvalidOperationException("Can not retrieve the error instanve from a success result.")
            : _error;

        protected internal Result(string message)
        {
            _message = string.IsNullOrWhiteSpace(message)
                ? string.Empty : message.Trim();

            IsSuccess = true;

            _error = null;
        }

        protected internal Result(Error error, string message)
        {
            _message = string.IsNullOrWhiteSpace(message)
                ? string.Empty : message.Trim();

            if (error is null)
            {
                IsSuccess = true;
                _error = null;
            }
            else
            {
                IsSuccess = false;
                _error = error;
            }
        }

        public static Result Success() => new(string.Empty);
        public static Result Success(string message) => new(message);
        public static Result<TValue> Success<TValue>(TValue value) where TValue : class => new(value, null);

        public static Result Failure() => new(Error.Default, null);
        public static Result Failure(Error error) => new(error, string.Empty);
        public static Result<TValue> Failure<TValue>() where TValue : class => new(null, Error.Default, string.Empty);
        public static Result<TValue> Failure<TValue>(Error error) where TValue : class => new(null, error, string.Empty);
    }

    public class Result<TValue> : Result where TValue : class
    {
        private readonly TValue _value;

        protected internal Result(TValue value, string message) : base(message)
        {
            _value = value;
        }

        protected internal Result(TValue value, Error error, string message) : base(error, message)
        {
            _value = value;
        }

        public TValue Value => IsFailure
            ? throw new InvalidOperationException("Can not retrieve the value from a failure result.")
            : _value;

        // NOTE: Is causing unexpected bahaviour
        // public static implicit operator TValue(Result<TValue> result) => result.Value;
        // public static implicit operator Result<TValue>(TValue value) => new(value, null, string.Empty);
    }
}
