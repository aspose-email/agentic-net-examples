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
            // Path to the MBOX file
            string mboxPath = "storage.mbox";

            // Verify that the MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Unique identifiers of the messages to retrieve
            string[] targetIds = new string[] { "id1", "id2" };

            // Create the MBOX reader
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                foreach (string id in targetIds)
                {
                    try
                    {
                        // Extract the message by its unique identifier
                        using (MailMessage mail = mboxReader.ExtractMessage(id, new EmlLoadOptions()))
                        {
                            Console.WriteLine($"Subject: {mail.Subject}");
                            Console.WriteLine($"From: {mail.From}");
                            // Additional processing can be performed here
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to extract message with ID '{id}': {ex.Message}");
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
