using System;
using Aspose.Email.Tools.Verifications;

namespace EmailValidationSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Determine the email address to validate
                string emailAddress = args.Length > 0 ? args[0] : "user@example.com";

                // Create an EmailValidator instance
                EmailValidator validator = new EmailValidator();

                // Perform validation using the default MailServer policy
                ValidationResult result;
                validator.Validate(emailAddress, out result);

                // Output validation outcome
                Console.WriteLine("Email Address: " + emailAddress);
                Console.WriteLine("Return Code: " + result.ReturnCode);
                Console.WriteLine("Message: " + result.Message);
                if (result.LastException != null)
                {
                    Console.WriteLine("Exception: " + result.LastException.Message);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}
