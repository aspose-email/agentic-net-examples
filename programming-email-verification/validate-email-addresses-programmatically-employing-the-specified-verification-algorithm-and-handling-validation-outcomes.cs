using Aspose.Email;
using System;
using Aspose.Email.Tools.Verifications;

namespace EmailValidationSample
{
    class Program
    {
        static void Main()
        {
            // Sample email addresses to validate
            string[] emailAddresses = new string[]
            {
                "valid.user@example.com",
                "invalid-email",
                "user@nonexistentdomain.xyz"
            };

            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            foreach (string address in emailAddresses)
            {
                try
                {
                    // Perform validation using the default MailServer policy
                    ValidationResult result;
                    validator.Validate(address, out result);

                    // Check the validation response code
                    if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
                    {
                        Console.WriteLine($"[Valid]   {address}");
                    }
                    else
                    {
                        Console.WriteLine($"[Invalid] {address} - Reason: {result.Message}");
                    }
                }
                catch (Exception ex)
                {
                    // Catch any unexpected errors during validation
                    Console.Error.WriteLine($"Error validating '{address}': {ex.Message}");
                }
            }
        }
    }
}
