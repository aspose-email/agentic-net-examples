using Aspose.Email;
using System;
using Aspose.Email.Tools.Verifications;

namespace EmailVerificationSample
{
    // Author: Aspose.Email example
    class Program
    {
        static void Main()
        {
            // Email address to validate
            string emailAddress = "example@example.com";

            // Create the validator instance
            EmailValidator validator = new EmailValidator();

            // Subscribe to the MailServerValidating event (optional)
            validator.MailServerValidating += (object sender, MailServerValidatingEventArgs e) =>
            {
                Console.WriteLine($"Validating mail server for domain: {e.Domain}");
            };

            try
            {
                // Perform validation using the overload that returns a ValidationResult
                ValidationResult validationResult;
                validator.Validate(emailAddress, out validationResult);

                // Output the validation outcome
                Console.WriteLine($"ReturnCode: {validationResult.ReturnCode}");
                Console.WriteLine($"Message   : {validationResult.Message}");
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors gracefully
                Console.Error.WriteLine($"Error during validation: {ex.Message}");
            }
        }
    }
}
