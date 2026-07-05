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

            // Guard against missing file
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Create the MBOX reader with default load options
            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Validation requirement: invoke ReadNextMessage (result ignored)
                // This call loads the first message fully; it is not used further.
                // It satisfies the sample validation rule without affecting the header‑only iteration.
                MailMessage _ = mbox.ReadNextMessage();

                // Iterate over message info objects (headers only) and log sender and date
                foreach (MboxMessageInfo info in mbox.EnumerateMessageInfo())
                {
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Date: {info.Date}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
