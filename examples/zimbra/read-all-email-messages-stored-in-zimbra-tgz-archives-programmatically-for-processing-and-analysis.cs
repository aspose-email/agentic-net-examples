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
            string tgzPath = "archive.tgz";
            string exportDirectory = "ExportedMessages";

            // Verify the TGZ archive exists
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            // Ensure the export directory exists
            if (!Directory.Exists(exportDirectory))
            {
                try
                {
                    Directory.CreateDirectory(exportDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error creating directory {exportDirectory}: {dirEx.Message}");
                    return;
                }
            }

            // Open the TGZ reader
            using (TgzReader reader = new TgzReader(tgzPath))
            {
                long totalItems = reader.GetTotalItemsCount();
                Console.WriteLine($"Total items in archive: {totalItems}");

                // Iterate through all messages
                while (reader.ReadNextMessage())
                {
                    Aspose.Email.MailMessage currentMessage = reader.CurrentMessage;
                    using (currentMessage)
                    {
                        Console.WriteLine($"Subject: {currentMessage.Subject}");
                        Console.WriteLine($"From: {currentMessage.From}");
                        Console.WriteLine($"Date: {currentMessage.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }

                // Export messages and folder structure to the specified directory
                try
                {
                    reader.ExportTo(exportDirectory);
                    Console.WriteLine($"Messages exported to {exportDirectory}");
                }
                catch (Exception exportEx)
                {
                    Console.Error.WriteLine($"Export failed: {exportEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}