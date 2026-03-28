using System;
using Aspose.Email.Tools.Verifications;

namespace EmailValidationSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Sample email addresses to validate
                string[] emails = new string[]
                {
                    "valid.email@example.com",
                    "invalid-email",
                    "user@nonexistentdomain.tld"
                };

                // Create an EmailValidator instance
                EmailValidator validator = new EmailValidator();

                foreach (string email in emails)
                {
                    // Validate the email address
                    ValidationResult result;
                    validator.Validate(email, out result);

                    // Report the validation outcome
                    Console.WriteLine($"Email: {email}");
                    Console.WriteLine($"Return Code: {result.ReturnCode}");
                    Console.WriteLine($"Message: {result.Message}");
                    if (result.LastException != null)
                    {
                        Console.WriteLine($"Exception: {result.LastException.Message}");
                    }
                    Console.WriteLine(new string('-', 40));
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
