using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Email address to be validated
            string emailAddress = "example@example.com";

            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            // Perform validation and capture the result
            ValidationResult validationResult;
            try
            {
                validator.Validate(emailAddress, out validationResult);
            }
            catch (AsposeException ex)
            {
                Console.Error.WriteLine($"Validation process threw an exception: {ex.Message}");
                return;
            }

            // Display validation details for UI (console) purposes
            Console.WriteLine($"Email: {emailAddress}");
            Console.WriteLine($"Return Code: {validationResult.ReturnCode}");
            Console.WriteLine($"Message: {validationResult.Message}");
            if (validationResult.LastException != null)
            {
                Console.WriteLine($"Last Exception: {validationResult.LastException.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
