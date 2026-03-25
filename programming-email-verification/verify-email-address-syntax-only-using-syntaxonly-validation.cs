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
            string emailAddress = "example@example.com";

            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            // Validate the email address using SyntaxOnly policy
            ValidationResult result;
            validator.Validate(emailAddress, ValidationPolicy.SyntaxOnly, out result);

            // Output the validation result
            Console.WriteLine("Validation Return Code: " + result.ReturnCode);
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