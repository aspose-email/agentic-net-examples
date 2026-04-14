using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Sample list of email addresses to validate
            List<string> emailAddresses = new List<string>
            {
                "valid@example.com",
                "invalid-email",
                "test@domain",
                "user@.com"
            };

            // Create an instance of EmailValidator
            Aspose.Email.Tools.Verifications.EmailValidator validator = new Aspose.Email.Tools.Verifications.EmailValidator();

            foreach (string email in emailAddresses)
            {
                // Validate the email address
                Aspose.Email.Tools.Verifications.ValidationResult result;
                validator.Validate(email, out result);

                // Check the validation result
                if (result.ReturnCode != Aspose.Email.Tools.Verifications.ValidationResponseCode.ValidationSuccess)
                {
                    Console.WriteLine($"Invalid email: {email} - {result.Message}");
                }
                else
                {
                    Console.WriteLine($"Valid email: {email}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
