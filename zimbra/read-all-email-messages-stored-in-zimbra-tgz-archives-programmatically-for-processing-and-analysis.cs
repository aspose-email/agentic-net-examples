using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the Zimbra TGZ archive
            string archivePath = "archive.tgz";

            // Verify that the archive file exists before proceeding
            if (!File.Exists(archivePath))
            {
                Console.Error.WriteLine($"Error: File not found – {archivePath}");
                return;
            }

            // Open the TGZ archive using TgzReader inside a using block to ensure disposal
            using (TgzReader reader = new TgzReader(archivePath))
            {
                // Optionally, display total number of items in the archive
                try
                {
                    int totalItems = reader.GetTotalItemsCount();
                    Console.WriteLine($"Total items in archive: {totalItems}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving total items: {ex.Message}");
                }

                // Iterate through each message in the archive
                while (true)
                {
                    bool hasMessage;
                    try
                    {
                        hasMessage = reader.ReadNextMessage();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error reading next message: {ex.Message}");
                        break;
                    }

                    if (!hasMessage)
                    {
                        // No more messages to read
                        break;
                    }

                    MailMessage currentMessage;
                    try
                    {
                        currentMessage = reader.CurrentMessage;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error accessing current message: {ex.Message}");
                        continue;
                    }

                    if (currentMessage != null)
                    {
                        // Output basic metadata for each message
                        string subject = currentMessage.Subject ?? "(No Subject)";
                        string from = currentMessage.From != null ? currentMessage.From.ToString() : "(Unknown Sender)";
                        Console.WriteLine($"Subject: {subject}");
                        Console.WriteLine($"From: {from}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}