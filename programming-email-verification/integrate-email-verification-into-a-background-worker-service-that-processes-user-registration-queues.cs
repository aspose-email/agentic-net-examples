using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Simulated user registration email queue
            List<string> registrationQueue = new List<string>
            {
                "valid@example.com",
                "invalid-email",
                "user@nonexistentdomain.xyz"
            };

            // Process each registration entry
            foreach (string email in registrationQueue)
            {
                ProcessRegistration(email);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled error: {ex.Message}");
        }
    }

    static void ProcessRegistration(string email)
    {
        // Create EmailValidator instance
        EmailValidator validator = new EmailValidator();

        // Perform validation
        ValidationResult result;
        try
        {
            validator.Validate(email, out result);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Validation exception for '{email}': {ex.Message}");
            return;
        }

        // Check validation result using ReturnCode
        if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
        {
            Console.WriteLine($"Email '{email}' is valid.");
            // TODO: Add further user registration logic here
        }
        else
        {
            Console.WriteLine($"Email '{email}' is invalid. Reason: {result.Message}");
        }
    }
}
