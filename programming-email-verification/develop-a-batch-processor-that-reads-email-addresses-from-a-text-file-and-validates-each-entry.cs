using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "emails.txt";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    File.WriteAllLines(inputPath, new[]
                    {
                        "valid@example.com",
                        "invalid-email",
                        "user@nonexistentdomain.xyz"
                    });
                    Console.WriteLine($"Placeholder file created at '{inputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            List<string> emailLines;
            try
            {
                emailLines = new List<string>(File.ReadAllLines(inputPath));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading file '{inputPath}': {ex.Message}");
                return;
            }

            EmailValidator validator = new EmailValidator();

            foreach (string line in emailLines)
            {
                string email = line.Trim();
                if (string.IsNullOrEmpty(email))
                    continue;

                ValidationResult result;
                try
                {
                    validator.Validate(email, out result);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Validation threw an exception for '{email}': {ex.Message}");
                    continue;
                }

                bool isValid = result.ReturnCode == ValidationResponseCode.ValidationSuccess;
                Console.WriteLine($"{email} => {(isValid ? "Valid" : "Invalid")} (Code: {result.ReturnCode}, Message: {result.Message})");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
