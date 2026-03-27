using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Email address to validate
            string email = "example@example.com";

            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            // Validate the email address using SyntaxOnly policy
            ValidationResult result;
            validator.Validate(email, ValidationPolicy.SyntaxOnly, out result);

            // Display the validation outcome
            Console.WriteLine("Return Code: " + result.ReturnCode);
            Console.WriteLine("Message: " + result.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
