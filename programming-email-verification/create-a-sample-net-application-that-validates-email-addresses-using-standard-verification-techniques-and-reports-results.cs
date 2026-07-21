using System;
using System.Text.RegularExpressions;

namespace EmailVerificationSample
{
    class Program
    {
        static void Main()
        {
            // Sample email addresses to validate
            string[] emailAddresses = new string[]
            {
                "john.doe@example.com",
                "invalid-email@@example.com",
                "jane_doe@sub.domain.org",
                "user@localhost",
                "bademail@.com"
            };

            // Create the validator instance
            EmailValidator validator = new EmailValidator();

            foreach (string address in emailAddresses)
            {
                ValidationResult result = validator.Validate(address);
                Console.WriteLine($"{address} => {(result.IsValid ? "Valid" : "Invalid")} - {result.Reason}");
            }
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Reason { get; set; }
    }

    public class EmailValidator
    {
        // Simple regex for basic email syntax validation
        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public ValidationResult Validate(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return new ValidationResult { IsValid = false, Reason = "Empty or whitespace" };

            if (!EmailRegex.IsMatch(email))
                return new ValidationResult { IsValid = false, Reason = "Invalid format" };

            // Split local part and domain
            var parts = email.Split('@');
            if (parts.Length != 2)
                return new ValidationResult { IsValid = false, Reason = "Incorrect '@' placement" };

            var domain = parts[1];

            // Additional simple domain checks
            if (domain.StartsWith("-") || domain.EndsWith("-"))
                return new ValidationResult { IsValid = false, Reason = "Domain starts or ends with hyphen" };

            if (domain.Contains(".."))
                return new ValidationResult { IsValid = false, Reason = "Domain contains consecutive dots" };

            // All checks passed
            return new ValidationResult { IsValid = true, Reason = "Syntax OK" };
        }
    }
}
