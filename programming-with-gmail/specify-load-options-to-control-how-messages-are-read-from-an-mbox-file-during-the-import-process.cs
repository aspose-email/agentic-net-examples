using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Define input MBOX file and output HTML file paths.
            string mboxPath = "input.mbox";
            string htmlPath = "firstMessage.html";

            // Guard against missing input file.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Create load options to control how messages are read.
            MboxLoadOptions loadOptions = new MboxLoadOptions
            {
                // Example: keep the underlying stream open after disposing the reader (optional).
                LeaveOpen = false,
                // Example: specify preferred encoding if needed (null uses default detection).
                PreferredTextEncoding = null
            };

            // Open the MBOX storage reader using the factory method.
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                // Read the first message sequentially.
                MailMessage message = reader.ReadNextMessage();

                if (message == null)
                {
                    Console.Error.WriteLine("No messages found in the MBOX file.");
                    return;
                }

                // Save the message as HTML.
                HtmlSaveOptions htmlOptions = new HtmlSaveOptions();
                message.Save(htmlPath, htmlOptions);
                Console.WriteLine($"First message saved to {htmlPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
