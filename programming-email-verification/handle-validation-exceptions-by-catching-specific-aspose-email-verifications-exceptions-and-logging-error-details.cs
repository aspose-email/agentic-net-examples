using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Email address to validate
            string emailAddress = "test@example.com";

            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            // Perform validation
            ValidationResult validationResult;
            validator.Validate(emailAddress, out validationResult);

            // Evaluate the validation result
            if (validationResult.ReturnCode == ValidationResponseCode.ValidationSuccess)
            {
                Console.WriteLine("The email address is valid.");
            }
            else
            {
                Console.WriteLine($"Validation failed. Return code: {validationResult.ReturnCode}");
                Console.WriteLine($"Message: {validationResult.Message}");
                if (validationResult.LastException != null)
                {
                    Console.WriteLine($"Exception: {validationResult.LastException.Message}");
                }
            }
        }
        catch (AsposeException ex)
        {
            Console.Error.WriteLine($"Aspose exception: {ex.Message}");
            if (ex.ErrorDetails != null)
            {
                Console.Error.WriteLine($"Details: {ex.ErrorDetails}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
