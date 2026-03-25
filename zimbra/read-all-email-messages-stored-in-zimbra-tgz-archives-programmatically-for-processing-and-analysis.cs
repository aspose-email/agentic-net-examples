using System.Collections.Generic;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        List<MailMessage> messages = new List<MailMessage>();

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
                int totalMessages = reader.GetTotalItemsCount();
                Console.WriteLine($"Total messages in archive: {totalMessages}");

                for (int i = 0; i < totalMessages; i++)
                {
                    try
                    {
                        reader.ReadNextMessage();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error reading message #{i + 1}: {ex.Message}");
                        continue;
                    }

                    Aspose.Email.MailMessage message = reader.CurrentMessage;
                    if (message == null)
                    {
                        continue;
                    }

                    Console.WriteLine($"Message #{i + 1}");
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.From}");
                    Console.WriteLine($"Date: {message.Date}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled error: {ex.Message}");
        }
    }
}