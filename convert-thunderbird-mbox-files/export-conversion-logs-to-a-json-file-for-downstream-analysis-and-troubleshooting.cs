using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailConversionLogExport
{
    // Represents a single conversion log entry.
    public class ConversionLog
    {
        public string MessageSubject { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define paths.
                string inputMboxPath = "input.mbox";
                string outputPstPath = "output.pst";
                string logJsonPath = "conversion_log.json";

                // Guard input file existence.
                if (!File.Exists(inputMboxPath))
                {
                    Console.Error.WriteLine($"Input MBOX file not found: {inputMboxPath}");
                    return;
                }

                // Ensure output directory exists.
                string outputDirectory = Path.GetDirectoryName(outputPstPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Prepare log collection.
                List<ConversionLog> logs = new List<ConversionLog>();

                // Define the mail handler to capture each message conversion.
                MailStorageConverter.MailHandler handler = delegate (MailMessage message)
                {
                    logs.Add(new ConversionLog
                    {
                        MessageSubject = message.Subject,
                        SourcePath = inputMboxPath,
                        DestinationPath = outputPstPath,
                        Timestamp = DateTime.Now,
                        Success = true,
                        ErrorMessage = null
                    });
                };

                // Perform conversion inside a using block to dispose the PST.
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(inputMboxPath, outputPstPath, handler))
                {
                    // No additional actions required; conversion occurs during the call.
                }

                // Serialize logs to JSON.
                string json = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });

                // Ensure log file directory exists.
                string logDirectory = Path.GetDirectoryName(logJsonPath);
                if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Write JSON to file with guard.
                try
                {
                    File.WriteAllText(logJsonPath, json);
                    Console.WriteLine($"Conversion log written to: {logJsonPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to write log file: {ioEx.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
