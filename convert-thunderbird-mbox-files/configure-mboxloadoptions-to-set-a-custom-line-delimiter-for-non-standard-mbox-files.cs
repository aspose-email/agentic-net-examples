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
            // Path to the MBOX file (replace with actual path)
            string mboxPath = "sample.mbox";

            // Verify that the file exists before attempting to read it
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Configure load options (no line delimiter property exists; using default settings)
            MboxLoadOptions loadOptions = new MboxLoadOptions
            {
                PreferredTextEncoding = System.Text.Encoding.UTF8,
                LeaveOpen = false
            };

            // Create a reader for the MBOX storage
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                MailMessage message;
                // Read messages sequentially
                while ((message = reader.ReadNextMessage()) != null)
                {
                    using (message)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Output any unexpected errors
            Console.Error.WriteLine(ex.Message);
        }
    }
}
