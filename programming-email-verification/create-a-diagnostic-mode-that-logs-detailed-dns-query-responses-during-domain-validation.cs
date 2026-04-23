using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Email address to validate
            string emailAddress = "test@example.com";

            // Create the EmailValidator instance
            EmailValidator validator = new EmailValidator();

            // Subscribe to the DomainValidating event to log DNS query details
            validator.DomainValidating += (object sender, DomainValidatingEventArgs e) =>
            {
                Console.WriteLine($"[DomainValidating] Domain: {e.Domain}");

                try
                {
                    // Perform a DNS lookup for the domain (A records)
                    IPHostEntry hostEntry = Dns.GetHostEntry(e.Domain);
                    Console.WriteLine($"  HostName: {hostEntry.HostName}");
                    foreach (IPAddress ip in hostEntry.AddressList)
                    {
                        Console.WriteLine($"  IP Address: {ip}");
                    }
                }
                catch (Exception dnsEx)
                {
                    Console.WriteLine($"  DNS query failed: {dnsEx.Message}");
                }
            };

            // Execute validation
            ValidationResult result;
            validator.Validate(emailAddress, out result);

            // Output the validation result
            Console.WriteLine($"Validation ReturnCode: {result.ReturnCode}");
            if (result.LastException != null)
            {
                Console.WriteLine($"Validation Exception: {result.LastException.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled error: {ex.Message}");
        }
    }
}
