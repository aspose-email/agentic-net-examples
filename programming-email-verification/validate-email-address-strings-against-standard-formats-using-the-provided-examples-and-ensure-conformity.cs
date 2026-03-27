using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Email address to validate
            string emailAddress = "user@example.com";

            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            // Perform validation using the default MailServer policy
            ValidationResult result;
            validator.Validate(emailAddress, out result);

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
