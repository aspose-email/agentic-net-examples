using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string tgzPath = "archive.tgz";

            // Verify that the TGZ file exists before attempting to read it
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            // Open the Zimbra TGZ archive using Aspose.Email's TgzReader
            using (Aspose.Email.Storage.Zimbra.TgzReader tgzReader = new Aspose.Email.Storage.Zimbra.TgzReader(tgzPath))
            {
                // Retrieve and display the total number of items in the archive
                long totalItems = tgzReader.GetTotalItemsCount();
                Console.WriteLine($"Total items in archive: {totalItems}");

                // Iterate through each message in the archive
                while (tgzReader.ReadNextMessage())
                {
                    // CurrentMessage returns an Aspose.Email.MailMessage instance
                    MailMessage mailMessage = tgzReader.CurrentMessage;

                    // Output basic metadata for analysis
                    Console.WriteLine($"Subject: {mailMessage.Subject}");
                    Console.WriteLine($"From: {mailMessage.From?.Address}");
                    Console.WriteLine($"Date: {mailMessage.Date}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard: report any unexpected errors without crashing
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}