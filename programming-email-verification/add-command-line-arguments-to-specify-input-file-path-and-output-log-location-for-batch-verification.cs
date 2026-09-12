using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main(string[] args)
    {
        if (args == null || args.Length < 2)
        {
            Console.Error.WriteLine("Usage: <program> <inputFilePath> <logFilePath>");
            return;
        }

        string inputFilePath = args[0];
        string logFilePath = args[1];

        if (!File.Exists(inputFilePath))
        {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

            Console.Error.WriteLine($"Input file not found: {inputFilePath}");
            try
            {
                string placeholderContent = "Subject: Placeholder\r\n\r\nBody";
                File.WriteAllText(inputFilePath, placeholderContent);
                Console.WriteLine("Created placeholder EML file.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                return;
            }
        }

        string logDirectory = Path.GetDirectoryName(logFilePath);
        if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
        {
            try
            {
                Directory.CreateDirectory(logDirectory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create log directory: {ex.Message}");
                return;
            }
        }

        var errors = new List<string>();
        try
        {
            var message = MailMessage.Load(inputFilePath);
        }
        catch (Exception ex)
        {
            errors.Add($"Failed to load/validate message: {ex.Message}");
        }

        try
        {
            using (var writer = new StreamWriter(logFilePath, false))
            {
                if (errors.Count == 0)
                {
                    writer.WriteLine("No validation errors found.");
                }
                else
                {
                    foreach (var err in errors)
                    {
                        writer.WriteLine($"Error: {err}");
                    }
                }
            }
            Console.WriteLine($"Validation results written to: {logFilePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to write log file: {ex.Message}");
        }
    }
}
