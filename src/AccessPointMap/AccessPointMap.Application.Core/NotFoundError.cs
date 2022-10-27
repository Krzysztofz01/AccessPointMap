namespace AccessPointMap.Application.Core
{
    public class NotFoundError : Error
    {
        const string _message = "No elements with matching specification found.";

        protected NotFoundError() { }
        protected NotFoundError(string message) : base(message) { }

        public static new NotFoundError Default => new(_message);
    }
}
