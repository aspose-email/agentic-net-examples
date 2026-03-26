using System;
using Aspose.Email.Tools.Verifications;

namespace AsposeEmailValidationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Email address to validate
                string emailAddress = "user@example.com";

                // Create an instance of EmailValidator
                EmailValidator validator = new EmailValidator();

                // Perform validation
                ValidationResult validationResult;
                validator.Validate(emailAddress, out validationResult);

                // Check validation result using the verified ReturnCode property
                if (validationResult.ReturnCode == ValidationResponseCode.ValidationSuccess)
                {
                    Console.WriteLine("The email address is valid.");
                }
                else
                {
                    Console.WriteLine("The email address is invalid. Reason: " + validationResult.ReturnCode);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}