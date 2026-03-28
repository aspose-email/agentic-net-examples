using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "storage.mbox";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Error: File not found – {mboxPath}");
                return;
            }

            MboxLoadOptions loadOptions = new MboxLoadOptions();
            loadOptions.PreferredTextEncoding = Encoding.UTF8;

            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                foreach (MboxMessageInfo messageInfo in reader.EnumerateMessageInfo())
                {
                    // Extract the full MIME message using the preferred encoding.
                    MailMessage message = reader.ExtractMessage(messageInfo.EntryId, new EmlLoadOptions());

                    // Example: output basic details.
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.From}");
                    Console.WriteLine($"To: {message.To}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
