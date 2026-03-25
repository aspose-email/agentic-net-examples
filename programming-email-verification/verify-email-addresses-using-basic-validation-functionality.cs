using System;
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
                "another.user@domain.co.uk"
            };

            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            foreach (string email in emailAddresses)
            {
                ValidationResult result;
                // Validate using syntax-only policy
                validator.Validate(email, ValidationPolicy.SyntaxOnly, out result);

                Console.WriteLine("Email: {0}", email);
                Console.WriteLine("Return Code: {0}", result.ReturnCode);
                if (!string.IsNullOrEmpty(result.Message))
                {
                    Console.WriteLine("Message: {0}", result.Message);
                }
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: {0}", ex.Message);
        }
    }
}