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

            // Create the validator instance
            EmailValidator validator = new EmailValidator();

            // Perform validation using SyntaxAndDomain policy
            ValidationResult validationResult;
            validator.Validate(emailAddress, ValidationPolicy.SyntaxAndDomain, out validationResult);

            // Output the validation result
            Console.WriteLine("ReturnCode: " + validationResult.ReturnCode);
            if (!string.IsNullOrEmpty(validationResult.Message))
            {
                Console.WriteLine("Message: " + validationResult.Message);
            }
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
