using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string inputFolder = "InputMails";

            // Ensure the input folder exists
            if (!Directory.Exists(inputFolder))
            {
                try
                {
                    Directory.CreateDirectory(inputFolder);
                    // Create a minimal placeholder EML file
                    string placeholderPath = Path.Combine(inputFolder, "placeholder.eml");
                    string minimalEml = "From: placeholder@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(placeholderPath, minimalEml);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to prepare input folder: {ioEx.Message}");
                    return;
                }
            }

            string[] emlFiles;
            try
            {
                emlFiles = Directory.GetFiles(inputFolder, "*.eml");
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to enumerate EML files: {dirEx.Message}");
                return;
            }

            if (emlFiles.Length == 0)
            {
                Console.WriteLine("No EML files found to process.");
                return;
            }

            List<Task> processingTasks = new List<Task>();
            foreach (string filePath in emlFiles)
            {
                processingTasks.Add(ProcessMessageAsync(filePath));
            }

            await Task.WhenAll(processingTasks);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static async Task ProcessMessageAsync(string filePath)
    {
        // Guard file existence
        if (!File.Exists(filePath))
        {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

            Console.Error.WriteLine($"File not found: {filePath}");
            return;
        }

        try
        {
            // Load the MIME message asynchronously using Task.Run to avoid blocking
            MailMessage message = await Task.Run(() => MailMessage.Load(filePath));

            // Ensure the MailMessage is disposed after processing
            using (message)
            {
                // Example processing: output subject and sender
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"From: {message.From}");
                Console.WriteLine($"Processed file: {Path.GetFileName(filePath)}");
                Console.WriteLine(new string('-', 40));
            }
        }
        catch (Exception loadEx)
        {
            Console.Error.WriteLine($"Failed to load or process '{filePath}': {loadEx.Message}");
        }
    }
}
