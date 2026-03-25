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

            // Open the TGZ archive using TgzReader inside a using block to ensure disposal
            using (Aspose.Email.Storage.Zimbra.TgzReader tgzReader = new Aspose.Email.Storage.Zimbra.TgzReader(archivePath))
            {
                // Optionally display the total number of items in the archive
                long totalItems = tgzReader.GetTotalItemsCount();
                Console.WriteLine($"Total items in archive: {totalItems}");

                // Iterate through each message in the archive
                while (tgzReader.ReadNextMessage())
                {
                    Aspose.Email.MailMessage message = tgzReader.CurrentMessage;
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
            // Output any unexpected errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}