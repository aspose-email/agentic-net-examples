using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Email address to validate; can be passed as a command‑line argument.
            string emailAddress = args.Length > 0 ? args[0] : "user@example.com";

            // Create the validator.
            EmailValidator validator = new EmailValidator();

            // Perform validation using the SyntaxAndDomain policy.
            ValidationResult validationResult;
            validator.Validate(emailAddress, ValidationPolicy.SyntaxAndDomain, out validationResult);

            // Output the validation outcome.
            Console.WriteLine("Return Code: " + validationResult.ReturnCode);
            Console.WriteLine("Message: " + validationResult.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}