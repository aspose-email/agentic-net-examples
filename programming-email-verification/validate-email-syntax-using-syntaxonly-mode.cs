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
            string emailAddress = "example@example.com";

            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            // Perform validation using SyntaxOnly policy
            ValidationResult validationResult;
            validator.Validate(emailAddress, ValidationPolicy.SyntaxOnly, out validationResult);

            // Output the validation result code and message
            Console.WriteLine("ReturnCode: " + validationResult.ReturnCode);
            if (!string.IsNullOrEmpty(validationResult.Message))
            {
                Console.WriteLine("Message: " + validationResult.Message);
            }
        }
        catch (Exception ex)
        {
            // Write any unexpected errors to the error console
            Console.Error.WriteLine(ex.Message);
        }
    }
}