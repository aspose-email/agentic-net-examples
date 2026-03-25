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

            // Create the validator instance
            EmailValidator validator = new EmailValidator();

            // Perform validation with syntax and domain checks
            ValidationResult validationResult;
            validator.Validate(emailAddress, ValidationPolicy.SyntaxAndDomain, out validationResult);

            // Output the validation result
            Console.WriteLine("Return Code: " + validationResult.ReturnCode);
            if (!string.IsNullOrEmpty(validationResult.Message))
            {
                Console.WriteLine("Message: " + validationResult.Message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}