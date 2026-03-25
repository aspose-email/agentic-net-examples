using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Email address to validate
            string emailAddress = "user@example.com";

            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            // Validate the email address using the MailServer validation policy (default overload)
            ValidationResult validationResult;
            validator.Validate(emailAddress, out validationResult);

            // Output the validation result
            Console.WriteLine("Validation Return Code: " + validationResult.ReturnCode);
            if (!string.IsNullOrEmpty(validationResult.Message))
            {
                Console.WriteLine("Message: " + validationResult.Message);
            }
            if (validationResult.LastException != null)
            {
                Console.WriteLine("Exception: " + validationResult.LastException.Message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}