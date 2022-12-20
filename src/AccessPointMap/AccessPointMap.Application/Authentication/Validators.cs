using FluentValidation;
using System;
using System.Text.RegularExpressions;
using static AccessPointMap.Application.Authentication.Requests.V1;

namespace AccessPointMap.Application.Authentication
{
    public static class Validators
    {
        public static class V1
        {
            public class LoginValidator : AbstractValidator<Login>
            {
                public LoginValidator()
                {
                    RuleFor(c => c.Email).NotEmpty().EmailAddress();
                    RuleFor(c => c.Password).NotEmpty();
                }
            }

            public class RegisterValidator : AbstractValidator<Register>
            {
                public RegisterValidator()
                {
                    RuleFor(c => c.Email).NotEmpty().EmailAddress();
                    RuleFor(c => c.Name).NotEmpty();

                    RuleFor(c => c.Password)
                        .NotEmpty()
                        .Must(IsPasswordFormatValid).WithMessage("The password is not matching the strength requirements.");

                    RuleFor(c => c.PasswordRepeat)
                        .NotEmpty()
                        .Must(IsPasswordFormatValid).WithMessage("The password is not matching the strength requirements.")
                        .Equal(c => c.Password).WithMessage(c => "The password and repeated password values do not match.");
                }
            }

            public class LogoutValidator : AbstractValidator<Logout>
            {
                public LogoutValidator()
                {
                    RuleFor(c => c.RefreshToken).NotEmpty();
                }
            }

            public class PasswordResetValidator : AbstractValidator<PasswordReset>
            {
                public PasswordResetValidator()
                {
                    RuleFor(c => c.Password)
                        .NotEmpty()
                        .Must(IsPasswordFormatValid).WithMessage("The password is not matching the strength requirements.");

                    RuleFor(c => c.PasswordRepeat)
                        .NotEmpty()
                        .Must(IsPasswordFormatValid).WithMessage("The password is not matching the strength requirements.")
                        .Equal(c => c.Password).WithMessage(c => "The password and repeated password values do not match.");
                }
            }

            public class RefreshValidator : AbstractValidator<Refresh>
            {
                public RefreshValidator()
                {
                    RuleFor(c => c.RefreshToken).NotEmpty();
                }
            }

            private static bool IsPasswordFormatValid(string value)
            {
                try
                {
                    if (value is null) return false;

                    return
                        Regex.IsMatch(value, @"[0-9]+", RegexOptions.Compiled, TimeSpan.FromSeconds(1)) &&
                        Regex.IsMatch(value, @"[A-Z]+", RegexOptions.Compiled, TimeSpan.FromSeconds(1)) &&
                        Regex.IsMatch(value, @".{8,}", RegexOptions.Compiled, TimeSpan.FromSeconds(1));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
