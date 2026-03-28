using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            // Email address to validate
            string email = "example@example.com";

            // Perform validation
            ValidationResult result;
            validator.Validate(email, out result);

            // Output validation result
            Console.WriteLine("Return Code: " + result.ReturnCode);
            Console.WriteLine("Message: " + result.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
