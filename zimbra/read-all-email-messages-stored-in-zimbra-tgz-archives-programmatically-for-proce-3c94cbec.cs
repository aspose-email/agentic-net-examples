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

            // Verify that the archive file exists before attempting to read it
            if (!File.Exists(archivePath))
            {
                Console.Error.WriteLine($"Error: File not found – {archivePath}");
                return;
            }

            // Initialize the TgzReader to read messages from the archive
            using (Aspose.Email.Storage.Zimbra.TgzReader reader = new Aspose.Email.Storage.Zimbra.TgzReader(archivePath))
            {
                // Iterate through all messages in the archive
                while (reader.ReadNextMessage())
                {
                    // CurrentMessage returns a MailMessage instance; ensure it is disposed after use
                    using (Aspose.Email.MailMessage message = reader.CurrentMessage)
                    {
                        // Output basic metadata for analysis
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From?.Address}");
                        Console.WriteLine($"Date: {message.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Gracefully handle any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}