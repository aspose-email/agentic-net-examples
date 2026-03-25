using System.Collections.Generic;
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
        List<MailMessage> messages = new List<MailMessage>();

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

                // Initialize the TgzReader inside a using block to ensure proper disposal
                using (Aspose.Email.Storage.Zimbra.TgzReader tgzReader = new Aspose.Email.Storage.Zimbra.TgzReader(tgzFilePath))
                {
                    int messageCounter = 0;

                    // Iterate through all messages in the archive
                    while (tgzReader.ReadNextMessage())
                    {
                        // CurrentMessage returns a MailMessage instance; dispose it after use
                        using (Aspose.Email.MailMessage mailMessage = tgzReader.CurrentMessage)
                        {
                            if (mailMessage != null)
                            {
                                messageCounter++;
                                Console.WriteLine($"Message {messageCounter}:");
                                Console.WriteLine($"Subject: {mailMessage.Subject}");
                                Console.WriteLine($"From: {mailMessage.From?.Address}");
                                Console.WriteLine($"Date: {mailMessage.Date}");
                                Console.WriteLine(new string('-', 40));
                            }
                        }
                    }

                    if (messageCounter == 0)
                    {
                        Console.WriteLine("No messages were found in the TGZ archive.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Output any unexpected errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}