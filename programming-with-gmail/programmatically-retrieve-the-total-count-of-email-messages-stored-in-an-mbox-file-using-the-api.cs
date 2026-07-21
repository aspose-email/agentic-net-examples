using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            const string mboxPath = "storage.mbox";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            int totalMessages = 0;

            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                MailMessage message;
                while ((message = reader.ReadNextMessage()) != null)
                {
                    totalMessages++;
                }
            }

            Console.WriteLine($"Total messages in MBOX: {totalMessages}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
