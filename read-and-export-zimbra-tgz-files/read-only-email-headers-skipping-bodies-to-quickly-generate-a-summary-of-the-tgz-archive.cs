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
                Console.Error.WriteLine($"File not found: {tgzPath}");
                return;
            }

            // Open the TGZ archive using TgzReader
            using (TgzReader tgzReader = new TgzReader(tgzPath))
            {
                // Get total number of items in the archive
                int totalItems = tgzReader.GetTotalItemsCount();
                Console.WriteLine($"Total items in archive: {totalItems}");

                // Iterate through each message in the TGZ archive
                while (tgzReader.ReadNextMessage())
                {
                    // Access the current message (headers only are used)
                    MailMessage currentMessage = tgzReader.CurrentMessage;

                    // Output selected header information
                    Console.WriteLine("----- Message Header -----");
                    Console.WriteLine($"Subject: {currentMessage.Subject}");
                    Console.WriteLine($"From: {currentMessage.From}");
                    Console.WriteLine($"Date: {currentMessage.Date}");
                    Console.WriteLine($"Message-ID: {currentMessage.MessageId}");

                    // Iterate through all custom headers
                    foreach (string headerName in currentMessage.Headers.Keys)
                    {
                        string headerValue = currentMessage.Headers[headerName];
                        Console.WriteLine($"{headerName}: {headerValue}");
                    }

                    Console.WriteLine("--------------------------");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
