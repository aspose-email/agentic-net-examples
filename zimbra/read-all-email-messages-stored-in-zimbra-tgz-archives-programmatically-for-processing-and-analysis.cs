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

            // Open the TGZ archive using TgzReader
            using (Aspose.Email.Storage.Zimbra.TgzReader reader = new Aspose.Email.Storage.Zimbra.TgzReader(tgzPath))
            {
                // Iterate through all messages in the archive
                while (true)
                {
                    try
                    {
                        // Read the next message; returns false when no more messages are available
                        bool hasMessage = reader.ReadNextMessage();
                        if (!hasMessage)
                        {
                            break;
                        }

                        // Retrieve the current message
                        Aspose.Email.MailMessage message = reader.CurrentMessage;

                        // Example processing: output basic metadata
                        string subject = message.Subject ?? "(no subject)";
                        string from = message.From != null ? message.From.ToString() : "(no sender)";
                        Console.WriteLine($"Subject: {subject}");
                        Console.WriteLine($"From: {from}");
                        Console.WriteLine($"Date: {message.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors that occur while processing an individual message
                        Console.Error.WriteLine($"Error reading message: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level exception handling to prevent crashes
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}