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

            // Ensure the MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (FileStream placeholderStream = new FileStream(mboxPath, FileMode.Create, FileAccess.Write))
                    {
                        string placeholderMessage = "From - Mon Jan 01 00:00:00 2020\r\nSubject: Placeholder\r\n\r\nThis is a placeholder message.\r\n";
                        byte[] bytes = Encoding.UTF8.GetBytes(placeholderMessage);
                        placeholderStream.Write(bytes, 0, bytes.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Configure load options with UTF-8 encoding
            MboxLoadOptions loadOptions = new MboxLoadOptions();
            loadOptions.PreferredTextEncoding = Encoding.UTF8;

            // Create the MBOX reader
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                // Read the first message
                MailMessage message = mboxReader.ReadNextMessage();
                if (message != null)
                {
                    using (message)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {message.To}");
                    }
                }
                else
                {
                    Console.WriteLine("No messages found in the MBOX file.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
