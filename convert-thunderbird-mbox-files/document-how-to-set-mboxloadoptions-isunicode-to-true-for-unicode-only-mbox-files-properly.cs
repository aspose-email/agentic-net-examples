using System;
using System.IO;
using System.Text;
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
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ioEx.Message}");
                    return;
                }
            }

            // Configure load options for Unicode‑only MBOX files.
            MboxLoadOptions loadOptions = new MboxLoadOptions
            {
                // Use Unicode encoding to correctly read Unicode messages.
                PreferredTextEncoding = Encoding.Unicode
            };

            // Create a reader for the MBOX storage with the specified options.
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                MailMessage message;
                // Read messages sequentially.
                while ((message = reader.ReadNextMessage()) != null)
                {
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
