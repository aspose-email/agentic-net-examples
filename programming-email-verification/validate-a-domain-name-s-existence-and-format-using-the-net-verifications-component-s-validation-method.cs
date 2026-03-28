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

            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            // Validate the email address using syntax and domain checking
            ValidationResult result;
            validator.Validate(email, ValidationPolicy.SyntaxAndDomain, out result);

            // Display validation outcome
            Console.WriteLine("Return Code: " + result.ReturnCode);
            if (!string.IsNullOrEmpty(result.Message))
            {
                Console.WriteLine("Message: " + result.Message);
            }
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
