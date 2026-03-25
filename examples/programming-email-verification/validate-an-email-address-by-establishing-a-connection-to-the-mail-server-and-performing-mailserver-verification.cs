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
                // Email address to validate
                string emailAddress = "example@example.com";

                // Create EmailValidator instance
                EmailValidator validator = new EmailValidator();

                // Perform validation with MailServer policy (default)
                ValidationResult result;
                validator.Validate(emailAddress, out result);

                // Output validation result
                Console.WriteLine("Return Code: " + result.ReturnCode);
                Console.WriteLine("Message: " + result.Message);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}