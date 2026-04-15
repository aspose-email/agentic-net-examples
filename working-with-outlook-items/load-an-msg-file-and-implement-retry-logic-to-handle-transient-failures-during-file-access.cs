using System;
using System.IO;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file
            string msgFilePath = "sample.msg";

            // Ensure the directory exists
            string directoryPath = Path.GetDirectoryName(msgFilePath);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{directoryPath}': {dirEx.Message}");
                    return;
                }
            }

            // If the file does not exist, create a minimal placeholder MSG
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder Message";
                        placeholder.Body = "This is a placeholder MSG file.";
                        placeholder.Save(msgFilePath);
                        Console.WriteLine($"Placeholder MSG created at '{msgFilePath}'.");
                    }
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {createEx.Message}");
                    return;
                }
            }

            // Retry logic parameters
            const int maxAttempts = 3;
            const int delayMilliseconds = 1000;
            int attempt = 0;
            MapiMessage loadedMessage = null;

            // Attempt to load the MSG file with retries
            while (attempt < maxAttempts)
            {
                try
                {
                    loadedMessage = MapiMessage.Load(msgFilePath);
                    break; // Success
                }
                catch (Exception loadEx)
                {
                    attempt++;
                    Console.Error.WriteLine($"Attempt {attempt} to load MSG failed: {loadEx.Message}");
                    if (attempt >= maxAttempts)
                    {
                        Console.Error.WriteLine("Maximum retry attempts reached. Exiting.");
                        return;
                    }
                    Thread.Sleep(delayMilliseconds);
                }
            }

            // Use the loaded message
            if (loadedMessage != null)
            {
                using (MapiMessage message = loadedMessage)
                {
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.SenderName}");
                    Console.WriteLine($"Body: {message.Body}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
