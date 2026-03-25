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
            string tgzFilePath = "archive.tgz";

            if (!File.Exists(tgzFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzFilePath}");
                return;
            }

            using (Aspose.Email.Storage.Zimbra.TgzReader tgzReader = new Aspose.Email.Storage.Zimbra.TgzReader(tgzFilePath))
            {
                int totalItems = tgzReader.GetTotalItemsCount();
                Console.WriteLine($"Total items in archive: {totalItems}");

                while (tgzReader.ReadNextMessage())
                {
                    using (Aspose.Email.MailMessage mailMessage = tgzReader.CurrentMessage)
                    {
                        Console.WriteLine($"Subject: {mailMessage.Subject}");
                        Console.WriteLine($"From: {mailMessage.From}");
                        Console.WriteLine($"Date: {mailMessage.Date}");
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