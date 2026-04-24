using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Tools.Verifications;

namespace EmailVerificationTests
{
    class Program
    {
        static void Main()
        {
            try
            {
                EmailValidator validator = new EmailValidator();

                List<string> validEmails = new List<string>
                {
                    "user@example.com",
                    "user.name+tag+sorting@example.co.uk",
                    "user_name@example.org"
                };

                List<string> invalidEmails = new List<string>
                {
                    "plainaddress",
                    "@missingusername.com",
                    "username@.com"
                };

                foreach (string email in validEmails)
                {
                    ValidationResult result;
                    validator.Validate(email, out result);
                    if (result.ReturnCode != ValidationResponseCode.ValidationSuccess)
                    {
                        Console.Error.WriteLine($"Valid email test failed for: {email}");
                        return;
                    }
                }

                foreach (string email in invalidEmails)
                {
                    ValidationResult result;
                    validator.Validate(email, out result);
                    if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
                    {
                        Console.Error.WriteLine($"Invalid email test failed for: {email}");
                        return;
                    }
                }

                Console.WriteLine("All email validation tests passed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
