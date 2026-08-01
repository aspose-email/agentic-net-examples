using Aspose.Email;
using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        // Email address to validate
        string email = "test@example.com";

        // Create the validator instance
        EmailValidator validator = new EmailValidator();

        // Perform syntax and domain validation
        ValidationResult validationResult;
        try
        {
            validator.Validate(email, ValidationPolicy.SyntaxAndDomain, out validationResult);

            bool isValid = validationResult.ReturnCode == (int)ValidationResponseCode.ValidationSuccess;
            Console.WriteLine($"IsValid: {isValid}");
            Console.WriteLine($"Message: {validationResult.Message}");
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors (e.g., DNS lookup failures)
            Console.Error.WriteLine($"Validation error: {ex.Message}");
        }
    }
}
