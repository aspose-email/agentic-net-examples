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
                string tgzFilePath = args.Length > 0 ? args[0] : "archive.tgz";

                if (!File.Exists(tgzFilePath))
                {
                    Console.Error.WriteLine($"Error: File not found – {tgzFilePath}");
                    return;
                }

                using (Aspose.Email.Storage.Zimbra.TgzReader tgzReader = new Aspose.Email.Storage.Zimbra.TgzReader(tgzFilePath))
                {
                    long totalItems = tgzReader.GetTotalItemsCount();
                    Console.WriteLine($"Total items in archive: {totalItems}");

                    while (tgzReader.ReadNextMessage())
                    {
                        Aspose.Email.MailMessage currentMessage = tgzReader.CurrentMessage;
                        if (currentMessage != null)
                        {
                            Console.WriteLine("----- Message -----");
                            Console.WriteLine($"Subject: {currentMessage.Subject}");
                            Console.WriteLine($"From: {currentMessage.From}");
                            Console.WriteLine($"Date: {currentMessage.Date}");
                            Console.WriteLine($"Body Preview: {GetBodyPreview(currentMessage.Body)}");
                            Console.WriteLine("-------------------");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        private static string GetBodyPreview(string body)
        {
            if (string.IsNullOrEmpty(body))
                return string.Empty;

            const int previewLength = 100;
            return body.Length <= previewLength ? body : body.Substring(0, previewLength) + "...";
        }
    }
}