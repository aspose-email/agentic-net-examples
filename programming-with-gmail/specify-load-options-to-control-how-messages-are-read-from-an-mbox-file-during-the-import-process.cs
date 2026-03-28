using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MBOX file
            string mboxFilePath = "sample.mbox";

            // Ensure the MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxFilePath))
            {
                try
                {
                    using (FileStream placeholderStream = File.Create(mboxFilePath))
                    {
                        // Create an empty MBOX file
                    }
                    Console.WriteLine($"Placeholder MBOX file created at '{mboxFilePath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ioEx.Message}");
                    return;
                }
            }

            // Configure load options for the MBOX reader
            MboxLoadOptions mboxLoadOptions = new MboxLoadOptions();

            // Configure EML load options that control how individual messages are parsed
            EmlLoadOptions emlLoadOptions = new EmlLoadOptions();

            // Open the MBOX reader with the specified load options
            try
            {
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxFilePath, mboxLoadOptions))
                {
                    // Enumerate messages using the EML load options
                    foreach (MailMessage message in mboxReader.EnumerateMessages(emlLoadOptions))
                    {
                        // Output basic information about each message
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {message.To}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception readerEx)
            {
                Console.Error.WriteLine($"Error reading MBOX file: {readerEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
