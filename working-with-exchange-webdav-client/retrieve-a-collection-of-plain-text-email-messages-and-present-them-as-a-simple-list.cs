using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            const string mboxPath = "storage.mbox";

            // Verify that the MBOX file exists before attempting to read it.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Create the MboxStorageReader.
            MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());

            // Iterate through each message info object in the MBOX storage.
            foreach (MboxMessageInfo mboxMessageInfo in mbox.EnumerateMessageInfo())
            {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine($"Subject: {mboxMessageInfo.Subject}");
                Console.WriteLine($"From: {mboxMessageInfo.From}");
                Console.WriteLine($"To: {mboxMessageInfo.To}");

                // Extract the full MIME message object from the MBOX storage.
                MailMessage eml = mbox.ExtractMessage(mboxMessageInfo.EntryId, new EmlLoadOptions());

                // Display the plain‑text body of the message, if available.
                string bodyText = eml.Body ?? string.Empty;
                Console.WriteLine("Body (plain text):");
                Console.WriteLine(bodyText);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
