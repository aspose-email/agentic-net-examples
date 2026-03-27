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

            // Guard against missing file
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Error: File not found – {mboxPath}");
                return;
            }

            // Specify the desired text encoding for loading MBOX messages
            MboxLoadOptions loadOptions = new MboxLoadOptions();
            loadOptions.PreferredTextEncoding = Encoding.UTF8;

            // Open the MBOX file with the specified options
            using (MboxrdStorageReader reader = new MboxrdStorageReader(mboxPath, loadOptions))
            {
                MailMessage message;
                // Read messages sequentially until none are left
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
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
