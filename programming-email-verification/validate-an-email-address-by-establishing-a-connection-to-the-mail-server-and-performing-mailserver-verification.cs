using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

namespace EmailVerificationSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Enter email address to validate: ");
                string emailAddress = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(emailAddress))
                {
                    Console.Error.WriteLine("Email address is empty.");
                    return;
                }

                // Create the validator
                EmailValidator validator = new EmailValidator();

                // Perform validation using the MailServer policy (default overload)
                ValidationResult result;
                validator.Validate(emailAddress, out result);

                // Check the validation result using the ReturnCode property
                if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
                {
                    Console.WriteLine("The email address is valid.");
                }
                else
                {
                    Console.WriteLine($"Validation failed. Reason: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
