using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // List of email addresses to validate
            string[] emailAddresses = new string[] { "test@example.com", "invalid-email", "user@domain.org" };
            int totalChecked = emailAddresses.Length;
            int validCount = 0;
            int invalidCount = 0;

            EmailValidator validator = new EmailValidator();

            foreach (string email in emailAddresses)
            {
                ValidationResult result;
                validator.Validate(email, out result);
                if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
                {
                    validCount++;
                }
                else
                {
                    invalidCount++;
                }
            }

            Console.WriteLine("Validation Summary:");
            Console.WriteLine($"Total Checked: {totalChecked}");
            Console.WriteLine($"Valid: {validCount}");
            Console.WriteLine($"Invalid: {invalidCount}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
