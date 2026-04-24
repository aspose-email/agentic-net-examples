using Aspose.Email;
using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Email address to validate
            string email = "user@example.com";

            // Primary DNS resolver list (placeholder)
            string[] primaryDns = new string[] { "8.8.8.8" };
            // Secondary DNS resolver list (placeholder)
            string[] secondaryDns = new string[] { "1.1.1.1" };

            // Create the validator
            EmailValidator validator = new EmailValidator();

            // Use primary DNS servers
            validator.DnsServers = primaryDns;

            // Perform validation with syntax and domain checks
            ValidationResult result;
            validator.Validate(email, ValidationPolicy.SyntaxAndDomain, out result);

            // If domain validation failed, try with secondary DNS servers
            if (result.ReturnCode == ValidationResponseCode.DomainValidationFailed)
            {
                Console.WriteLine("Primary DNS validation failed. Switching to secondary DNS resolver...");

                validator.DnsServers = secondaryDns;
                validator.Validate(email, ValidationPolicy.SyntaxAndDomain, out result);
            }

            // Output the final validation result
            Console.WriteLine($"Validation Return Code: {result.ReturnCode}");
            Console.WriteLine($"Message: {result.Message}");
            if (result.LastException != null)
            {
                Console.WriteLine($"Exception: {result.LastException.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
