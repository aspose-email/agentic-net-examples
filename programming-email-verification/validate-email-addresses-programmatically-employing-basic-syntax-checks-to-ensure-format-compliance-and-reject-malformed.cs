using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            // List of email addresses to validate
            string[] emailAddresses = new string[]
            {
                "valid.user@example.com",
                "invalid-email",
                "another.valid+tag@sub.domain.org",
                "bad@domain",
                "user@.com"
            };

            foreach (string email in emailAddresses)
            {
                ValidationResult result;
                validator.Validate(email, out result);

                Console.WriteLine($"Email: {email}");
                if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
                {
                    Console.WriteLine("  Status: Valid");
                }
                else if (result.ReturnCode == ValidationResponseCode.SyntaxValidationFailed)
                {
                    Console.WriteLine("  Status: Invalid syntax");
                }
                else if (result.ReturnCode == ValidationResponseCode.DomainValidationFailed)
                {
                    Console.WriteLine("  Status: Invalid domain");
                }
                else
                {
                    Console.WriteLine($"  Status: Validation error (Code: {result.ReturnCode})");
                }

                if (!string.IsNullOrEmpty(result.Message))
                {
                    Console.WriteLine($"  Details: {result.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
