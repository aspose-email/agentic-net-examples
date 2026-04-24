using Aspose.Email;
using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            // Configure custom DNS servers for domain verification
            validator.DnsServers = new string[] { "1.1.1.1", "8.8.8.8" };

            // Optional: set a timeout (in milliseconds)
            validator.Timeout = 5000;

            // Email address to validate
            string emailAddress = "example@example.com";

            // Perform validation
            ValidationResult validationResult;
            validator.Validate(emailAddress, out validationResult);

            // Evaluate the result using ReturnCode
            if (validationResult.ReturnCode == ValidationResponseCode.ValidationSuccess)
            {
                Console.WriteLine("The email address is valid.");
            }
            else
            {
                Console.WriteLine($"Email validation failed: {validationResult.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
