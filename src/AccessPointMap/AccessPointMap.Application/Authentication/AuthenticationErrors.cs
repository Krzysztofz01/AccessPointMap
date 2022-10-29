using AccessPointMap.Application.Core;

namespace AccessPointMap.Application.Authentication
{
    public static class AuthenticationErrors
    {
        public class AuthenticationCredentialsError : Error
        {
            protected AuthenticationCredentialsError(string message) : base(message) { }

            public static AuthenticationCredentialsError IdentityWithEmailAlreadyExist => new("Provded authentication credentials are already in use.");
            public static AuthenticationCredentialsError CredentialsNotMatching => new("Provided authentication credentials are not matching.");
            public static AuthenticationCredentialsError IdentityWithEmailNotFound => InvalidCredentialsToIdentity;
            public static AuthenticationCredentialsError IdentityAccessWithWrongPassword => InvalidCredentialsToIdentity;
            public static AuthenticationCredentialsError InvalidRefreshToken => InvalidCredentialsToIdentity;
            public static AuthenticationCredentialsError InvalidCredentialsToIdentity => new("Identity with given credentials not found.");
        }

        public class AuthenticationIdentityError : Error
        {
            protected AuthenticationIdentityError(string message) : base(message) { }

            public static AuthenticationIdentityError UserNotFound => new("Can not access identity represented by given identifiers.");
        }
    }
}
