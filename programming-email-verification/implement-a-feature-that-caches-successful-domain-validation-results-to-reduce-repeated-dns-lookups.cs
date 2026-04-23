using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Tools.Verifications;

class Program
{
    // Cache for successful domain validation results
    private static readonly Dictionary<string, ValidationResult> _domainCache = new Dictionary<string, ValidationResult>(StringComparer.OrdinalIgnoreCase);

    static void Main()
    {
        try
        {
            // Create the EmailValidator instance
            EmailValidator validator = new EmailValidator();

            // Subscribe to the DomainValidating event to use the cache
            validator.DomainValidating += (sender, e) =>
            {
                // If we already have a result for this domain, reuse it and skip DNS lookup
                if (_domainCache.TryGetValue(e.Domain, out ValidationResult cachedResult))
                {
                    e.Result = cachedResult;
                    e.Skip = true;
                }
            };

            // Email address to validate
            string emailAddress = "user@example.com";

            // Perform validation (uses MailServer validation policy by default)
            ValidationResult result;
            validator.Validate(emailAddress, out result);

            // Output the validation outcome
            Console.WriteLine($"Validation result for '{emailAddress}': {result.ReturnCode}");
            Console.WriteLine($"Message: {result.Message}");

            // If validation succeeded, cache the domain result for future calls
            if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
            {
                string domain = emailAddress.Substring(emailAddress.IndexOf('@') + 1);
                // Store a copy of the result to avoid accidental modifications
                _domainCache[domain] = new ValidationResult(result.ReturnCode);
                Console.WriteLine($"Domain '{domain}' cached for future validations.");
            }
        }
        catch (Exception ex)
        {
            // Write any unexpected errors to the error console
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
