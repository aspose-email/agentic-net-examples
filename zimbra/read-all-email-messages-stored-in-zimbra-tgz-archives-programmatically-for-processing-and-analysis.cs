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

            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            using (Aspose.Email.Storage.Zimbra.TgzReader reader = new Aspose.Email.Storage.Zimbra.TgzReader(tgzPath))
            {
                while (true)
                {
                    try
                    {
                        bool hasMessage = reader.ReadNextMessage();
                        if (!hasMessage)
                        {
                            break;
                        }

                        using (Aspose.Email.MailMessage message = reader.CurrentMessage)
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From?.DisplayName ?? message.From?.Address}");
                            Console.WriteLine($"Date: {message.Date}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing a message: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}