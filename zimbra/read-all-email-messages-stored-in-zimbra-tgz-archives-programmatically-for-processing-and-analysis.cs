using System.Collections.Generic;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        List<MailMessage> messages = new List<MailMessage>();

        try
        {
            string tgzPath = "archive.tgz";
            string outputDirectory = "ExtractedMessages";

            // Verify the TGZ archive exists
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error creating output directory: {dirEx.Message}");
                return;
            }

            // Open the TGZ reader
            using (TgzReader tgzReader = new TgzReader(tgzPath))
            {
                // Get total number of items
                int totalItems = tgzReader.GetTotalItemsCount();
                Console.WriteLine($"Total messages in archive: {totalItems}");

                // Iterate through each message
                while (tgzReader.ReadNextMessage())
                {
                    MailMessage currentMessage = tgzReader.CurrentMessage;
                    if (currentMessage != null)
                    {
                        Console.WriteLine("----- Message -----");
                        Console.WriteLine($"Subject: {currentMessage.Subject}");
                        Console.WriteLine($"From: {currentMessage.From?.DisplayName ?? currentMessage.From?.Address}");
                        Console.WriteLine($"Date: {currentMessage.Date}");
                        Console.WriteLine($"Body Preview: {currentMessage.Body?.Substring(0, Math.Min(100, currentMessage.Body.Length))}");
                        Console.WriteLine("-------------------");
                    }
                }

                // Optional: export all messages to the output directory
                try
                {
                    tgzReader.ExportTo(outputDirectory);
                    Console.WriteLine($"All messages exported to: {outputDirectory}");
                }
                catch (Exception exportEx)
                {
                    Console.Error.WriteLine($"Error exporting messages: {exportEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}