using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the sample EML file and the report output
            string emlPath = "sample.eml";
            string reportPath = "validation_report.txt";

            // Ensure the EML file exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(emlPath, false))
                    {
                        writer.WriteLine("From: sender@example.com");
                        writer.WriteLine("To: recipient@example.com");
                        writer.WriteLine("Subject: Test");
                        writer.WriteLine();
                        writer.WriteLine("This is a test email.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Collections to hold validation statistics
            int totalMessages = 0;
            int successfulMessages = 0;
            List<string> errorLines = new List<string>();

            // Validate the EML message
            try
            {
                MessageValidationResult msgResult = MessageValidator.Validate(emlPath);
                totalMessages++;

                if (msgResult.IsSuccess)
                {
                    successfulMessages++;
                }
                else
                {
                    errorLines.Add($"Message validation failed: {msgResult.ErrorMessage}");
                    if (msgResult.Errors != null)
                    {
                        foreach (var err in msgResult.Errors)
                        {
                            errorLines.Add($" - {err}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception during message validation: {ex.Message}");
                return;
            }

            // Example email address validation
            EmailValidator emailValidator = new EmailValidator();
            string[] emailsToCheck = { "valid.user@example.com", "invalid-email@", "user@nonexistent.tld" };
            foreach (string email in emailsToCheck)
            {
                try
                {
                    emailValidator.Validate(email, out ValidationResult result);
                    if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
                    {
                        successfulMessages++;
                    }
                    else
                    {
                        errorLines.Add($"Email '{email}' validation error: {result.Message}");
                    }
                    totalMessages++;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Exception during email validation for '{email}': {ex.Message}");
                }
            }

            // Write a simple report
            try
            {
                using (StreamWriter reportWriter = new StreamWriter(reportPath, false))
                {
                    reportWriter.WriteLine($"Total items validated: {totalMessages}");
                    reportWriter.WriteLine($"Successful validations: {successfulMessages}");
                    reportWriter.WriteLine($"Failed validations: {totalMessages - successfulMessages}");
                    if (errorLines.Count > 0)
                    {
                        reportWriter.WriteLine("Error details:");
                        foreach (string line in errorLines)
                        {
                            reportWriter.WriteLine(line);
                        }
                    }
                }
                Console.WriteLine($"Validation report written to '{reportPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write report: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
