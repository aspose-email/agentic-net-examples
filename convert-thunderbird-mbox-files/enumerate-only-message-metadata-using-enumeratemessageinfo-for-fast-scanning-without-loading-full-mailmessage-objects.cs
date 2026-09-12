using Aspose.Email;
using System;
using System.IO;
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

            // Create the MboxStorageReader instance.
            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Enumerate only the message metadata without loading full messages.
                foreach (MboxMessageInfo messageInfo in mbox.EnumerateMessageInfo())
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}");
                    Console.WriteLine($"From: {messageInfo.From}");
                    Console.WriteLine($"To: {messageInfo.To}");
                    Console.WriteLine(); // Blank line for readability
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
