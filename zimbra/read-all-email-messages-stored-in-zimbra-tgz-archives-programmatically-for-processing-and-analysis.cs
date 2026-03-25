using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            string tgzPath = "archive.tgz";

            // Verify the TGZ file exists before attempting to read it
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            // Open the TGZ archive using TgzReader inside a using block for proper disposal
            using (TgzReader reader = new TgzReader(tgzPath))
            {
                // Loop through all messages in the archive
                while (true)
                {
                    // Read the next message; if there are no more messages, CurrentMessage will be null
                    reader.ReadNextMessage();

                    MailMessage currentMessage = reader.CurrentMessage;
                    if (currentMessage == null)
                    {
                        // No more messages to process
                        break;
                    }

                    // Output basic metadata for each message
                    Console.WriteLine($"Subject: {currentMessage.Subject}");
                    Console.WriteLine($"From: {currentMessage.From}");
                    Console.WriteLine($"Date: {currentMessage.Date}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception: {ex.Message}");
        }
    }
}