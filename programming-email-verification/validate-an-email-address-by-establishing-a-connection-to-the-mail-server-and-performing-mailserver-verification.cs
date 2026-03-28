using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Email address to validate (replace with the address you want to check)
            string emailAddress = "example@example.com";

            // Create the validator
            EmailValidator validator = new EmailValidator();

            // Perform validation using the default MailServer validation policy
            ValidationResult result;
            validator.Validate(emailAddress, out result);

            // Check the validation result using the ReturnCode property
            if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
            {
                Console.WriteLine($"The email address '{emailAddress}' is valid.");
            }
            else
            {
                Console.WriteLine($"The email address '{emailAddress}' is invalid. Reason: {result.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
