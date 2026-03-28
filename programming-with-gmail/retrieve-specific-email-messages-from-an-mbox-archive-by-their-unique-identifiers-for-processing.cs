using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "archive.mbox";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // List of message EntryIds to retrieve
            List<string> targetIds = new List<string>
            {
                "id1",
                "id2"
            };

            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                foreach (string id in targetIds)
                {
                    try
                    {
                        MailMessage message = mbox.ExtractMessage(id, new EmlLoadOptions());
                        Console.WriteLine($"Message ID: {id}");
                        Console.WriteLine($"Subject: {message.Subject}");
                        // Additional processing can be done here
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to extract message with ID {id}: {ex.Message}");
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
