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

            // Verify the TGZ archive exists before attempting to open it
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            // Open the Zimbra TGZ archive
            using (Aspose.Email.Storage.Zimbra.TgzReader reader = new Aspose.Email.Storage.Zimbra.TgzReader(tgzPath))
            {
                while (true)
                {
                    // Attempt to read the next message; break when no more messages are available
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
                        break;
                    }

                    // Retrieve the current message and output basic metadata
                    Aspose.Email.MailMessage message = reader.CurrentMessage;
                    if (message != null)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"Date: {message.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}