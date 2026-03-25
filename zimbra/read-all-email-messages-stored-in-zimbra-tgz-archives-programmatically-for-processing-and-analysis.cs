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

            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            using (TgzReader reader = new TgzReader(tgzPath))
            {
                int totalItems = reader.GetTotalItemsCount();
                Console.WriteLine($"Total items in archive: {totalItems}");

                while (reader.ReadNextMessage())
                {
                    using (MailMessage message = reader.CurrentMessage)
                    {
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
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}