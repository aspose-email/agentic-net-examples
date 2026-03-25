using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Email address to validate
            string emailAddress = "example@example.com";

            // Create the validator
            EmailValidator validator = new EmailValidator();

            // Perform validation with SyntaxAndDomain policy
            ValidationResult validationResult;
            validator.Validate(emailAddress, ValidationPolicy.SyntaxAndDomain, out validationResult);

            // Output the validation result
            Console.WriteLine("Return code: " + validationResult.ReturnCode);
            Console.WriteLine("Message: " + validationResult.Message);
            if (validationResult.LastException != null)
            {
                Console.WriteLine("Exception: " + validationResult.LastException.Message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}