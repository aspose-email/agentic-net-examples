using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

namespace EmailVerificationSample
{
    public static class EmailVerificationHelper
    {
        public static ValidationResult ValidateEmailDomain(string emailAddress)
        {
            EmailValidator validator = new EmailValidator();
            ValidationResult result;
            validator.Validate(emailAddress, ValidationPolicy.SyntaxAndDomain, out result);
            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string email = "example@example.com";
                ValidationResult validationResult = EmailVerificationHelper.ValidateEmailDomain(email);
                Console.WriteLine($"Validation Return Code: {validationResult.ReturnCode}");
                Console.WriteLine($"Message: {validationResult.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
