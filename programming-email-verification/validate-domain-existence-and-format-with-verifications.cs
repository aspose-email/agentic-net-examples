using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Email address to validate
            string emailAddress = "user@example.com";

            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            // Perform validation (uses MailServer validation policy by default)
            ValidationResult validationResult;
            validator.Validate(emailAddress, out validationResult);

            // Output validation result
            Console.WriteLine("Validation Return Code: " + validationResult.ReturnCode);
            Console.WriteLine("Validation Message: " + validationResult.Message);
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors gracefully
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}