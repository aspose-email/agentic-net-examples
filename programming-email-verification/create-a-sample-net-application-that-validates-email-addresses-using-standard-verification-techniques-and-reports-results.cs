using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Sample email addresses to validate
            string[] emails = new string[] { "valid@example.com", "invalid-email", "user@nonexistentdomain.tld" };

            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            foreach (string email in emails)
            {
                // Perform validation using the default MailServer policy
                ValidationResult result;
                validator.Validate(email, out result);

                // Report the validation outcome
                Console.WriteLine($"Email: {email}");
                Console.WriteLine($"Return Code: {result.ReturnCode}");
                Console.WriteLine($"Message: {result.Message}");
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
