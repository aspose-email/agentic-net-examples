using Aspose.Email;
using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        // Sample email addresses to validate
        string[] emailAddresses = new string[]
        {
            "test@example.com",
            "invalid-email",
            "user@nonexistentdomain.xyz"
        };

        EmailValidator validator = new EmailValidator();

        foreach (string email in emailAddresses)
        {
            ValidationResult result;
            try
            {
                // Validate the email address using the out-parameter overload
                validator.Validate(email, out result);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception while validating '{email}': {ex.Message}");
                continue;
            }

            bool isValid = result.ReturnCode == ValidationResponseCode.ValidationSuccess;

            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"  IsValid: {isValid}");
            Console.WriteLine($"  Message: {result.Message}");
        }
    }
}
