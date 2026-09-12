using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using System.Text.Json;

namespace AsposeEmailConversionLogExport
{
    // Author: Generated example for exporting conversion logs to JSON
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define file paths
                string inputPath = "TestEml.eml";
                string outputPath = "output.msg";
                string logPath = "conversion.log";
                string jsonPath = "conversion_log.json";

                // Ensure input file exists; create a minimal placeholder if missing
                if (!File.Exists(inputPath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    string placeholderEml = "From: example@example.com\r\nTo: recipient@example.com\r\nSubject: Test\r\n\r\nThis is a test email.";
                    File.WriteAllText(inputPath, placeholderEml);
                }

                // Initialize log file (clear any existing content)
                File.WriteAllText(logPath, string.Empty);

                // Set up EML load options
                EmlLoadOptions emlLoadOptions = new EmlLoadOptions()
                {
                    PreserveTnefAttachments = true,
                    PreserveEmbeddedMessageFormat = true
                };

                // Load the EML message and convert it to MSG
                using (MailMessage message = MailMessage.Load(inputPath, emlLoadOptions))
                {
                    message.Save(outputPath, SaveOptions.DefaultMsg);
                    string successLog = $"[{DateTime.UtcNow:u}] Conversion succeeded from {inputPath} to {outputPath}{Environment.NewLine}";
                    File.AppendAllText(logPath, successLog);
                }

                // Read the log file lines
                string[] logLines = File.ReadAllLines(logPath);
                List<string> logEntries = new List<string>(logLines);

                // Serialize log entries to JSON with indentation
                JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(logEntries, jsonOptions);

                // Write JSON to the output file
                File.WriteAllText(jsonPath, json);

                Console.WriteLine("Conversion completed. Log exported to " + jsonPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
