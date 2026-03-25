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
                string tgzFilePath = "archive.tgz";

                if (!File.Exists(tgzFilePath))
                {
                    Console.Error.WriteLine($"Error: File not found – {tgzFilePath}");
                    return;
                }

                using (Aspose.Email.Storage.Zimbra.TgzReader tgzReader = new Aspose.Email.Storage.Zimbra.TgzReader(tgzFilePath))
                {
                    while (tgzReader.ReadNextMessage())
                    {
                        Aspose.Email.MailMessage mailMessage = tgzReader.CurrentMessage;
                        Console.WriteLine($"Subject: {mailMessage.Subject}");
                        Console.WriteLine($"From: {mailMessage.From}");
                        Console.WriteLine($"Date: {mailMessage.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}