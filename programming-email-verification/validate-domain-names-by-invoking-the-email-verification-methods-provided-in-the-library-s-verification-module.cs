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
            string[] emails = new string[]
            {
                "test@example.com",
                "invalid@@example.com",
                "user@nonexistentdomain.tld"
            };

            // Create an instance of EmailValidator
            EmailValidator emailValidator = new EmailValidator();

            foreach (string email in emails)
            {
                ValidationResult result;
                // Validate the email address using the default MailServer validation policy
                emailValidator.Validate(email, out result);

                Console.WriteLine($"Email: {email}");
                Console.WriteLine($"Result Code: {result.ReturnCode}");
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
