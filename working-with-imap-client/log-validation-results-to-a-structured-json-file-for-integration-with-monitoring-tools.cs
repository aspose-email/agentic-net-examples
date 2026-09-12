using System;
using System.IO;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ImapValidationSample
{
    // Simple DTO for JSON output
    public class ValidationResult
    {
        public bool CredentialsValid { get; set; }
        public int MessageCount { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Placeholder connection settings – replace with real values for actual runs
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                // Skip external call when placeholders are detected
                if (host.Contains("example") || username.Contains("example") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected – skipping IMAP operations.");
                    return;
                }

                // Prepare result object
                ValidationResult result = new ValidationResult
                {
                    CredentialsValid = false,
                    MessageCount = 0,
                    ErrorMessage = null
                };

                // Use ImapClient within a using block to ensure disposal
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    try
                    {
                        // Validate credentials
                        bool isValid = client.ValidateCredentials();
                        result.CredentialsValid = isValid;

                        if (isValid)
                        {
                            // Retrieve message count (list messages in default folder)
                            Aspose.Email.Clients.Imap.ImapMessageInfoCollection messages = client.ListMessages();
                            result.MessageCount = messages?.Count ?? 0;
                        }
                    }
                    catch (ImapException imapEx)
                    {
                        result.ErrorMessage = $"IMAP error: {imapEx.Message}";
                        Console.Error.WriteLine(result.ErrorMessage);
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = $"Unexpected error: {ex.Message}";
                        Console.Error.WriteLine(result.ErrorMessage);
                    }
                }

                // Serialize result to JSON
                string json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });

                // Define output file path
                string outputPath = "validation_result.json";

                // Ensure directory exists (if a directory is part of the path)
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    try
                    {
                        Directory.CreateDirectory(directory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create directory '{directory}': {dirEx.Message}");
                        return;
                    }
                }

                // Write JSON to file with error handling
                try
                {
                    File.WriteAllText(outputPath, json);
                    Console.WriteLine($"Validation result written to '{outputPath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to write JSON file: {ioEx.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
