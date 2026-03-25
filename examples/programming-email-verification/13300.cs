using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Create an instance of the EmailValidator
            EmailValidator validator = new EmailValidator();

            // Email address to be validated
            string emailAddress = "test@example.com";

            // Validate the email address using the default MailServer validation policy
            ValidationResult validationResult;
            validator.Validate(emailAddress, out validationResult);

            // Output the validation result
            Console.WriteLine("Return Code: " + validationResult.ReturnCode);
            Console.WriteLine("Message: " + validationResult.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}