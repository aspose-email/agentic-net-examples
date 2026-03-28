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
                "john.doe@example.com",
                "invalid-email",
                "jane@domain",
                "user@@example.com"
            };

            EmailValidator validator = new EmailValidator();

            foreach (string address in emailAddresses)
            {
                ValidationResult result;
                validator.Validate(address, out result);

                Console.WriteLine($"Email: {address}");
                Console.WriteLine($"ReturnCode: {result.ReturnCode}");
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
