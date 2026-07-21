using Aspose.Email;
using System;
using Aspose.Email.Tools.Verifications;

namespace EmailDomainValidationExample
{
    class Program
    {
        static void Main()
        {
            // Email address to validate
            string emailAddress = "user@example.com";

            // Create the validator instance
            EmailValidator validator = new EmailValidator();

            try
            {
                // Perform validation (syntax + domain) and obtain the result
                ValidationResult validationResult;
                validator.Validate(emailAddress, out validationResult);

                // Determine if the validation succeeded
                bool isValid = validationResult.ReturnCode == ValidationResponseCode.ValidationSuccess;

                Console.WriteLine($"Email address: {emailAddress}");
                Console.WriteLine($"Is valid: {isValid}");
                Console.WriteLine($"Message: {validationResult.Message}");
            }
            catch (Exception ex)
            {
                // Handle unexpected errors gracefully
                Console.Error.WriteLine($"An error occurred during validation: {ex.Message}");
            }
        }
    }
}
