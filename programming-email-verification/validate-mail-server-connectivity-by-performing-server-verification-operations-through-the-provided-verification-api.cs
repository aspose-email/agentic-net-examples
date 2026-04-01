using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Create an instance of the email validator.
            EmailValidator validator = new EmailValidator();

            // Email address to be validated.
            string emailAddress = "test@example.com";

            // Perform validation using the default MailServer validation policy.
            ValidationResult result;
            validator.Validate(emailAddress, out result);

            // Check the validation result using the ReturnCode property.
            if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
            {
                Console.WriteLine("The email address is valid and the mail server is reachable.");
            }
            else
            {
                Console.WriteLine($"Validation failed. Reason: {result.Message}");
                if (result.LastException != null)
                {
                    Console.WriteLine($"Exception: {result.LastException.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}
