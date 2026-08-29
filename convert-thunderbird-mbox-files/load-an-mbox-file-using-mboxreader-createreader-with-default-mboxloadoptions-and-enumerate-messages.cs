// Author: Aspose.Email example - Load MBOX and enumerate messages
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        // Path to the MBOX file (adjust as needed)
        string mboxPath = Path.Combine(Environment.CurrentDirectory, "sample.mbox");

        // Guard against missing file
        if (!File.Exists(mboxPath))
        {
            Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
            return;
        }

        try
        {
            // Create the MBOX reader with default load options
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                MailMessage message;
                // Read messages sequentially
                while ((message = reader.ReadNextMessage()) != null)
                {
                    using (message)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {message.To}");
                        Console.WriteLine();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing MBOX: {ex.Message}");
        }
    }
}
