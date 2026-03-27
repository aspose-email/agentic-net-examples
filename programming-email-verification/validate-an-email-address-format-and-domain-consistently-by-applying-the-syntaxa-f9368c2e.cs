using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Email address to validate
            string emailAddress = "example@example.com";

            // Create the validator
            EmailValidator validator = new EmailValidator();

            // Perform validation with SyntaxAndDomain policy
            ValidationResult result;
            validator.Validate(emailAddress, ValidationPolicy.SyntaxAndDomain, out result);

            // Output the validation result
            Console.WriteLine("Return Code: " + result.ReturnCode);
            Console.WriteLine("Message: " + result.Message);
            if (result.LastException != null)
            {
                Console.WriteLine("Exception: " + result.LastException.Message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}