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
            string[] emailAddresses = new string[]
            {
                "user@example.com",
                "invalid-email",
                "test@domain"
            };

            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            foreach (string address in emailAddresses)
            {
                // Perform validation using the default policy (MailServer validation)
                ValidationResult result;
                validator.Validate(address, out result);

                Console.WriteLine($"Email: {address}");
                Console.WriteLine($"Return Code: {result.ReturnCode}");
                if (!string.IsNullOrEmpty(result.Message))
                {
                    Console.WriteLine($"Message: {result.Message}");
                }
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
