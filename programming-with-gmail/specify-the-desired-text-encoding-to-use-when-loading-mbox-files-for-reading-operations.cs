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
    try
    {
        string placeholderMbox = "From placeholder@example.com Sat Jan 01 00:00:00 2026\n";
        File.WriteAllText(mboxPath, placeholderMbox);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MBOX: {ex.Message}");
        return;
    }
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
