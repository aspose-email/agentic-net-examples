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

            // Create the validator (EmailValidator is not IDisposable)
            EmailValidator validator = new EmailValidator();

            // Perform validation
            ValidationResult result;
            validator.Validate(emailAddress, out result);

            // Output validation result
            Console.WriteLine("ReturnCode: " + result.ReturnCode);
            if (!string.IsNullOrEmpty(result.Message))
            {
                Console.WriteLine("Message: " + result.Message);
            }
            if (result.LastException != null)
            {
                Console.Error.WriteLine("Error: " + result.LastException.Message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}