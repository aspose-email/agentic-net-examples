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
                // Sample email addresses to validate
                string[] emailAddresses = new string[]
                {
                    "john.doe@example.com",
                    "invalid-email@@example.com",
                    "jane_doe@sub.domain.org",
                    "user@localhost",
                    "bademail@.com"
                };

                // Create an instance of the EmailValidator
                EmailValidator validator = new EmailValidator();

                foreach (string address in emailAddresses)
                {
                    // Validate the email address using the default MailServer validation policy
                    ValidationResult result;
                    validator.Validate(address, out result);

                    // Output the validation result
                    Console.WriteLine("Email: {0}", address);
                    Console.WriteLine("Return Code: {0}", result.ReturnCode);
                    if (!string.IsNullOrEmpty(result.Message))
                    {
                        Console.WriteLine("Message: {0}", result.Message);
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An error occurred: {0}", ex.Message);
                return;
            }
        }
    }
}