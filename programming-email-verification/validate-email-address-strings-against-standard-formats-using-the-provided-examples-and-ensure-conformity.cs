using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            string[] emails = { "valid@example.com", "invalid-email", "test@nonexistentdomain.xyz" };

            EmailValidator validator = new EmailValidator();

            foreach (string email in emails)
            {
                ValidationResult result;
                validator.Validate(email, out result);

                Console.WriteLine($"Email: {email}");
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
