using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Email address to validate
            string email = "example@example.com";

            // Create the validator
            EmailValidator validator = new EmailValidator();

            // Perform validation using the default MailServer policy
            ValidationResult result;
            validator.Validate(email, out result);

            // Output validation results
            Console.WriteLine($"ReturnCode: {result.ReturnCode}");
            Console.WriteLine($"Message: {result.Message}");

            // If an exception occurred during validation, display its details
            if (result.LastException != null)
            {
                Console.WriteLine($"Exception: {result.LastException.Message}");
            }
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors gracefully
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
