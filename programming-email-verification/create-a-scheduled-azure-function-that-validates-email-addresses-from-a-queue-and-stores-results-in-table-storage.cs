using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

namespace EmailValidationFunction
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Simulated queue of email addresses
                List<string> emailQueue = new List<string>
                {
                    "valid.user@example.com",
                    "invalid-email",
                    "another.valid@domain.org"
                };

                // Output CSV file path for storing validation results
                string outputPath = "validation_results.csv";

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {dirEx.Message}");
                        return;
                    }
                }

                // Create the CSV file with header if it does not exist
                if (!File.Exists(outputPath))
                {
                    try
                    {
                        using (StreamWriter headerWriter = new StreamWriter(outputPath, false))
                        {
                            headerWriter.WriteLine("EmailAddress,ReturnCode,Message");
                        }
                    }
                    catch (Exception createEx)
                    {
                        Console.Error.WriteLine($"Failed to create file '{outputPath}': {createEx.Message}");
                        return;
                    }
                }

                // Initialize the email validator
                EmailValidator validator = new EmailValidator();

                // Process each email address from the queue
                foreach (string emailAddress in emailQueue)
                {
                    try
                    {
                        // Validate the email address using default policy (syntax + domain)
                        validator.Validate(emailAddress, out ValidationResult validationResult);

                        // Prepare CSV line with the result
                        string csvLine = $"{emailAddress},{validationResult.ReturnCode},{validationResult.Message}";

                        // Append the result to the CSV file
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(outputPath, true))
                            {
                                writer.WriteLine(csvLine);
                            }
                        }
                        catch (Exception writeEx)
                        {
                            Console.Error.WriteLine($"Failed to write result for '{emailAddress}': {writeEx.Message}");
                        }
                    }
                    catch (Exception validateEx)
                    {
                        Console.Error.WriteLine($"Validation error for '{emailAddress}': {validateEx.Message}");
                    }
                }

                Console.WriteLine("Email validation processing completed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
