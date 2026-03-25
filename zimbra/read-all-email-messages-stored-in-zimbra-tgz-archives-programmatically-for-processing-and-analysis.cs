using System.Collections.Generic;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        List<MailMessage> messages = new List<MailMessage>();

        try
        {
            string tgzPath = "archive.tgz";
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            string outputDirectory = "ExtractedMessages";
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

            using (Aspose.Email.Storage.Zimbra.TgzReader reader = new Aspose.Email.Storage.Zimbra.TgzReader(tgzPath))
            {
                try
                {
                    long totalItems = reader.GetTotalItemsCount();
                    Console.WriteLine($"Total messages in archive: {totalItems}");
                }
                catch (Exception countEx)
                {
                    Console.Error.WriteLine($"Error retrieving total item count: {countEx.Message}");
                }

                // Optional: extract all messages to a folder
                try
                {
                    reader.ExportTo(outputDirectory);
                    Console.WriteLine($"All messages exported to: {outputDirectory}");
                }
                catch (Exception exportEx)
                {
                    Console.Error.WriteLine($"Error exporting messages: {exportEx.Message}");
                }

                // Iterate through messages
                while (true)
                {
                    try
                    {
                        // Attempt to read the next message
                        reader.ReadNextMessage();

                        MailMessage currentMessage = reader.CurrentMessage;
                        if (currentMessage == null)
                        {
                            // No more messages
                            break;
                        }

                        using (currentMessage)
                        {
                            Console.WriteLine("----- Message -----");
                            Console.WriteLine($"Subject: {currentMessage.Subject}");
                            Console.WriteLine($"From: {currentMessage.From}");
                            Console.WriteLine($"Date: {currentMessage.Date}");
                            Console.WriteLine($"Body Preview: {currentMessage.Body?.Substring(0, Math.Min(100, currentMessage.Body?.Length ?? 0))}");
                            Console.WriteLine("-------------------");
                        }
                    }
                    catch (Exception readEx)
                    {
                        // Assume end of archive or read error
                        Console.Error.WriteLine($"Error reading message: {readEx.Message}");
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}