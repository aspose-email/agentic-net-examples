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

            // Verify that the TGZ file exists
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            // Open the TGZ reader inside a using block to ensure disposal
            using (Aspose.Email.Storage.Zimbra.TgzReader reader = new Aspose.Email.Storage.Zimbra.TgzReader(tgzPath))
            {
                // Loop through all messages in the archive
                while (true)
                {
                    try
                    {
                        // Attempt to read the next message; if no more messages, break
                        reader.ReadNextMessage();
                    }
                    catch (Exception readEx)
                    {
                        // If reading fails because there are no more messages, exit the loop
                        // Otherwise, report the error and continue
                        Console.Error.WriteLine($"Read error: {readEx.Message}");
                        break;
                    }

                    // Retrieve the current message
                    MailMessage currentMessage = reader.CurrentMessage;
                    if (currentMessage == null)
                    {
                        // No more messages available
                        break;
                    }

                    // Output basic metadata for analysis
                    Console.WriteLine($"Subject: {currentMessage.Subject}");
                    if (currentMessage.From != null)
                    {
                        Console.WriteLine($"From: {currentMessage.From.Address}");
                    }
                    Console.WriteLine($"Date: {currentMessage.Date}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}