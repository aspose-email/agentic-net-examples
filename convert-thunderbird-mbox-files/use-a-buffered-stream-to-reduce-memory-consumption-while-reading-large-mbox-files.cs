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
            const string mboxPath = "large.mbox";

            // Ensure the MBOX file exists; create an empty placeholder if missing.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (FileStream placeholderStream = File.Create(mboxPath))
                    {
                        // Write a minimal empty MBOX (no messages).
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Open the MBOX file with a buffered stream to reduce memory usage.
            try
            {
                using (FileStream fileStream = new FileStream(mboxPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (BufferedStream bufferedStream = new BufferedStream(fileStream))
                using (MboxStorageReader reader = MboxStorageReader.CreateReader(bufferedStream, new MboxLoadOptions()))
                {
                    MailMessage message;
                    while ((message = reader.ReadNextMessage()) != null)
                    {
                        using (message)
                        {
                            // Example processing: output basic information.
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"To: {message.To}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading MBOX file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
