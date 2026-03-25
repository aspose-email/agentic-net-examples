using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

namespace ZimbraTgzReaderExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the Zimbra TGZ archive
                string tgzFilePath = "archive.tgz";

                // Verify that the file exists before attempting to read it
                if (!File.Exists(tgzFilePath))
                {
                    Console.Error.WriteLine($"Error: File not found – {tgzFilePath}");
                    return;
                }

                // Initialize the TgzReader with the archive path
                using (TgzReader tgzReader = new TgzReader(tgzFilePath))
                {
                    // Iterate through all messages in the archive
                    while (tgzReader.ReadNextMessage())
                    {
                        // CurrentMessage returns a MailMessage instance
                        using (MailMessage mailMessage = tgzReader.CurrentMessage)
                        {
                            // Output basic metadata for each message
                            Console.WriteLine($"Subject: {mailMessage.Subject}");
                            Console.WriteLine($"From: {mailMessage.From}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors gracefully
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}