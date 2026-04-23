using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Tools.Verifications;

namespace EmailValidationCsvLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define the CSV output file path
                string csvPath = "validation_results.csv";

                // Ensure the directory for the CSV file exists
                try
                {
                    string directory = Path.GetDirectoryName(csvPath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                    return;
                }

                // Prepare a list of email addresses to validate
                List<string> emailAddresses = new List<string>
                {
                    "valid@example.com",
                    "invalid-email",
                    "user@nonexistentdomain.tld"
                };

                // Create the email validator instance
                EmailValidator validator = new EmailValidator();

                // Open the CSV file for appending
                try
                {
                    using (StreamWriter writer = new StreamWriter(csvPath, true))
                    {
                        // Write header if the file is new or empty
                        if (writer.BaseStream.Length == 0)
                        {
                            writer.WriteLine("EmailAddress,IsValid,TimestampUtc");
                        }

                        // Validate each email and log the result
                        foreach (string email in emailAddresses)
                        {
                            ValidationResult result;
                            validator.Validate(email, out result);

                            bool isValid = result.ReturnCode == ValidationResponseCode.ValidationSuccess;
                            string timestamp = DateTime.UtcNow.ToString("o"); // ISO 8601 format

                            writer.WriteLine($"{email},{isValid},{timestamp}");
                        }
                    }
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"File I/O error while writing CSV: {ioEx.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
