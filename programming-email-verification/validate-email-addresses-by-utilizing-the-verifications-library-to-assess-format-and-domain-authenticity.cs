using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

namespace EmailValidationSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Create an instance of the EmailValidator.
                EmailValidator validator = new EmailValidator();

                // Optional: configure DNS servers and timeout (in milliseconds).
                // validator.DnsServers = new string[] { "8.8.8.8", "8.8.4.4" };
                // validator.Timeout = 5000;

                // List of email addresses to validate.
                string[] emailAddresses = new string[]
                {
                    "alice@example.com",
                    "bob@invalid-domain",
                    "not-an-email"
                };

                foreach (string address in emailAddresses)
                {
                    // Perform validation using the default MailServer policy.
                    ValidationResult result;
                    validator.Validate(address, out result);

                    // Output the validation result.
                    Console.WriteLine($"{address} => {result}");
                }
            }
            catch (Exception ex)
            {
                // Gracefully report any unexpected errors.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
