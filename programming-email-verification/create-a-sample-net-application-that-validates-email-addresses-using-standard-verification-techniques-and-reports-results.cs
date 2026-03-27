using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            EmailValidator validator = new EmailValidator();

            string[] emailAddresses = new string[]
            {
                "valid@example.com",
                "invalid@@example.com",
                "user@nonexistentdomain.xyz"
            };

            foreach (string email in emailAddresses)
            {
                ValidationResult result;
                validator.Validate(email, out result);

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
