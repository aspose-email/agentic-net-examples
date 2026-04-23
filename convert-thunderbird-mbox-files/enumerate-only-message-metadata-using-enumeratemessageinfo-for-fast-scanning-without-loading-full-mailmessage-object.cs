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
            string mboxPath = "sample.mbox";

            // Ensure the MBOX file exists; create an empty placeholder if missing.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Load the MBOX storage using the factory method.
            MboxLoadOptions loadOptions = new MboxLoadOptions();

            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                // Enumerate message metadata without loading full MailMessage objects.
                foreach (MboxMessageInfo info in reader.EnumerateMessageInfo())
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Date: {info.Date}");
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
