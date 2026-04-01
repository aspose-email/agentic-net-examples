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
            string[] emailAddresses = new string[] { "user@example.com", "invalid-email", "test@nonexistentdomain.xyz" };

            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            foreach (string address in emailAddresses)
            {
                ValidationResult validationResult;
                validator.Validate(address, out validationResult);

                Console.WriteLine($"Email: {address}");
                Console.WriteLine($"Return Code: {validationResult.ReturnCode}");
                if (!string.IsNullOrEmpty(validationResult.Message))
                {
                    Console.WriteLine($"Message: {validationResult.Message}");
                }
                if (validationResult.LastException != null)
                {
                    Console.WriteLine($"Exception: {validationResult.LastException.Message}");
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
