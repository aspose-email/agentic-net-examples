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
                // Create an instance of the EmailValidator
                EmailValidator validator = new EmailValidator();

                // Sample email addresses to validate
                string[] emailAddresses = new string[]
                {
                    "valid@example.com",
                    "invalid@@example.com",
                    "user@nonexistentdomain.xyz"
                };

                // Validate each email address and report the result
                foreach (string address in emailAddresses)
                {
                    ValidationResult result;
                    validator.Validate(address, out result);

                    Console.WriteLine("Email: {0}", address);
                    Console.WriteLine("  ReturnCode: {0}", result.ReturnCode);
                    Console.WriteLine("  Message: {0}", result.Message);
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