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
            string tgzPath = "archive.tgz";
            // Destination folder for extracted messages
            string outputFolder = "ExtractedMessages";

            // Verify input file exists
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error creating output directory: {dirEx.Message}");
                    return;
                }
            }

            // Open the TGZ reader and process messages
            using (TgzReader tgzReader = new TgzReader(tgzPath))
            {
                try
                {
                    // Export all messages and directory structure to the output folder
                    tgzReader.ExportTo(outputFolder);
                }
                catch (Exception exportEx)
                {
                    Console.Error.WriteLine($"Error during export: {exportEx.Message}");
                    return;
                }

                // Iterate through messages if additional processing is needed
                while (true)
                {
                    try
                    {
                        // Read next message; break if no more messages
                        if (!tgzReader.ReadNextMessage())
                        {
                            break;
                        }
                    }
                    catch (Exception readEx)
                    {
                        Console.Error.WriteLine($"Error reading next message: {readEx.Message}");
                        break;
                    }

                    MailMessage currentMessage = tgzReader.CurrentMessage;
                    if (currentMessage != null)
                    {
                        // Example: display subject of each message
                        Console.WriteLine($"Subject: {currentMessage.Subject}");
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