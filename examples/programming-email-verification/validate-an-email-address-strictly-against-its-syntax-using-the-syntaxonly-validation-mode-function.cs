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
            string email = "example@example.com";

            // Create the validator
            EmailValidator validator = new EmailValidator();

            // Perform syntax-only validation
            ValidationResult result;
            validator.Validate(email, ValidationPolicy.SyntaxOnly, out result);

            // Output validation result
            Console.WriteLine("ReturnCode: " + result.ReturnCode);
            if (!string.IsNullOrEmpty(result.Message))
            {
                Console.WriteLine("Message: " + result.Message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}