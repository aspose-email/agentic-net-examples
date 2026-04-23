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
            string mboxPath = "sample.mbox";

            // Ensure the MBOX file exists; create an empty placeholder if missing.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                    Console.Error.WriteLine($"MBOX file not found. Created empty placeholder at '{mboxPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
                return;
            }

            // Open the MBOX storage reader with proper load options.
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                MailMessage message;
                // Read messages sequentially.
                while ((message = reader.ReadNextMessage()) != null)
                {
                    using (message)
                    {
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
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
