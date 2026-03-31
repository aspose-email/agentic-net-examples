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
            // Define output MBOX file path
            string mboxFilePath = "output.mbox";

            // Ensure the directory for the MBOX file exists
            string directoryPath = Path.GetDirectoryName(Path.GetFullPath(mboxFilePath));
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{directoryPath}': {dirEx.Message}");
                    return;
                }
            }

            // Prepare a sample email message
            MailMessage message = new MailMessage();
            message.From = new MailAddress("sender@example.com", "Sender Name");
            message.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
            message.Subject = "Sample MBOX Message";
            message.Body = "This is a test message written to an MBOX file using Aspose.Email.";

            // Configure MBOX save options (optional)
            MboxSaveOptions saveOptions = new MboxSaveOptions();
            saveOptions.FromShouldBeEscaped = true; // Escape "From " lines in the body if present
            saveOptions.LeaveOpen = false; // Close the underlying stream after disposing the writer

            // Write the message to the MBOX file
            try
            {
                using (MboxrdStorageWriter writer = new MboxrdStorageWriter(mboxFilePath, saveOptions))
                {
                    writer.WriteMessage(message);
                }

                Console.WriteLine($"Message successfully written to '{mboxFilePath}'.");
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to write message to MBOX file: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
