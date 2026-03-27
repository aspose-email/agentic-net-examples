using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;

namespace AsposeEmailMboxLoadOptionsExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Path to the MBOX file
                string mboxFilePath = "sample.mbox";

                // Verify that the file exists before attempting to read it
                if (!File.Exists(mboxFilePath))
                {
                    Console.Error.WriteLine($"Error: File not found – {mboxFilePath}");
                    return;
                }

                // Configure load options for reading the MBOX file
                MboxLoadOptions loadOptions = new MboxLoadOptions();
                loadOptions.LeaveOpen = false;                     // Do not keep the underlying stream open after disposal
                loadOptions.PreferredTextEncoding = Encoding.UTF8; // Use UTF-8 for message text decoding

                // Create a reader for the MBOX storage with the specified options
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxFilePath, loadOptions))
                {
                    // Example: extract a message by its entry identifier (replace with a real ID as needed)
                    // The ExtractMessage method requires an identifier and EmlLoadOptions.
                    // Here we demonstrate the call signature without actual execution.
                    // EmlLoadOptions emlOptions = new EmlLoadOptions();
                    // MailMessage message = mboxReader.ExtractMessage("some-message-id", emlOptions);
                    // Console.WriteLine($"Subject: {message.Subject}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
