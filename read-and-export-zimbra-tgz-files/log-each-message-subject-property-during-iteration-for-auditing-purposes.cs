using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

namespace AsposeEmailZimbraExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                const string tgzFilePath = "mailbox.tgz";

                // Verify that the TGZ file exists before attempting to read it.
                if (!File.Exists(tgzFilePath))
                {
                    Console.Error.WriteLine($"File not found: {tgzFilePath}");
                    return;
                }

                // Open the TGZ reader inside a using block to ensure proper disposal.
                using (TgzReader reader = new TgzReader(tgzFilePath))
                {
                    // Iterate through all messages in the TGZ archive.
                    // ReadNextMessage returns true while there are more messages.
                    while (reader.ReadNextMessage())
                    {
                        // CurrentMessage provides the MailMessage instance for the current entry.
                        MailMessage currentMessage = reader.CurrentMessage;

                        // Log the subject of each message for auditing purposes.
                        Console.WriteLine($"Subject: {currentMessage.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Capture any unexpected errors and write them to the error stream.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
