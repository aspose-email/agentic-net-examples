using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EmailVerificationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Sample email addresses to validate
            var emailSamples = new List<string>
            {
                "john.doe@example.com",
                "jane_doe123@sub.domain.co.uk",
                "invalid-email@",
                "another.invalid@domain",
                "user+mailbox/department=shipping@example.com",
                "plainaddress",
                "email@123.123.123.123",
                "email@[123.123.123.123]",
                "\"quoted@local\"@example.com",
                "very.common@example.com"
            };

            Console.WriteLine("Email Validation Results:");
            Console.WriteLine(new string('=', 30));

            foreach (var email in emailSamples)
            {
                bool isValid = IsValidEmail(email);
                Console.WriteLine($"{email} => {(isValid ? "Valid" : "Invalid")}");
            }
        }

        // Simple email validation using a regular expression that covers most common cases.
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // This pattern validates the general structure of an email address.
            const string pattern = @"^[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@"
                                 + @"(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z]{2,}$";

            return Regex.IsMatch(email, pattern, RegexOptions.Compiled);
        }
    }
}
