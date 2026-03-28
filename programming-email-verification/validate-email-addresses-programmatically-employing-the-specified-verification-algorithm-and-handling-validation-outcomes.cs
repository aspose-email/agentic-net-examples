using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the email validator
            EmailValidator validator = new EmailValidator();

            // List of email addresses to validate
            string[] emailAddresses = new string[]
            {
                "valid@example.com",
                "invalid-email",
                "user@nonexistentdomain.xyz"
            };

            foreach (string address in emailAddresses)
            {
                // Perform validation
                ValidationResult result;
                validator.Validate(address, out result);

                // Display the outcome
                Console.WriteLine($"Email: {address}");
                Console.WriteLine($"Return Code: {result.ReturnCode}");
                if (!string.IsNullOrEmpty(result.Message))
                {
                    Console.WriteLine($"Message: {result.Message}");
                }
                if (result.LastException != null)
                {
                    Console.WriteLine($"Exception: {result.LastException.Message}");
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
