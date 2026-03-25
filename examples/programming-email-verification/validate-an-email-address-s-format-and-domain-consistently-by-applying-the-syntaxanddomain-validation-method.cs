using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

namespace EmailValidationSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Email address to validate
                string emailAddress = "example@example.com";

                // Create the validator instance
                EmailValidator validator = new EmailValidator();

                // Perform validation with SyntaxAndDomain policy
                ValidationResult validationResult;
                validator.Validate(emailAddress, ValidationPolicy.SyntaxAndDomain, out validationResult);

                // Output the validation result
                Console.WriteLine("Return Code: " + validationResult.ReturnCode);
                if (!string.IsNullOrEmpty(validationResult.Message))
                {
                    Console.WriteLine("Message: " + validationResult.Message);
                }
                if (validationResult.LastException != null)
                {
                    Console.Error.WriteLine("Exception: " + validationResult.LastException.Message);
                }
            }
            catch (Exception ex)
            {
                // Global exception handling
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}