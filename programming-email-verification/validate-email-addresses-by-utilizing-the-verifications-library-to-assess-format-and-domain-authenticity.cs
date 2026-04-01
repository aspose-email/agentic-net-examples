using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Sample email address to validate
            string emailAddress = "example@example.com";

            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            // Perform validation
            ValidationResult validationResult;
            validator.Validate(emailAddress, out validationResult);

            // Output validation result
            Console.WriteLine("Return Code: " + validationResult.ReturnCode);
            Console.WriteLine("Message: " + validationResult.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
