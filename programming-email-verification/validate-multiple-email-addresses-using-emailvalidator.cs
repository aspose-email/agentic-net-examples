using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Create an instance of the EmailValidator
            Aspose.Email.Tools.Verifications.EmailValidator validator = new Aspose.Email.Tools.Verifications.EmailValidator();

            // Sample email addresses to validate
            string[] emailAddresses = new string[]
            {
                "valid.user@example.com",
                "invalid-email",
                "user@nonexistentdomain.xyz"
            };

            foreach (string address in emailAddresses)
            {
                // Perform validation using the default MailServer policy
                Aspose.Email.Tools.Verifications.ValidationResult result;
                validator.Validate(address, out result);

                // Output the validation outcome
                Console.WriteLine($"Email: {address}");
                if (result.ReturnCode == Aspose.Email.Tools.Verifications.ValidationResponseCode.ValidationSuccess)
                {
                    Console.WriteLine("  Status: Valid");
                }
                else
                {
                    Console.WriteLine($"  Status: Invalid (Code: {result.ReturnCode})");
                }

                // Additional details
                if (!string.IsNullOrEmpty(result.Message))
                {
                    Console.WriteLine($"  Message: {result.Message}");
                }

                if (result.LastException != null)
                {
                    Console.WriteLine($"  Exception: {result.LastException.Message}");
                }

                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            // Gracefully handle any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}