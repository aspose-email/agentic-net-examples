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
            // Define the output MBOX file path.
            string mboxFilePath = "output.mbox";

            // Ensure the directory for the MBOX file exists.
            string directoryPath = Path.GetDirectoryName(mboxFilePath);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Guard against any I/O errors when creating the writer.
            try
            {
                // Initialize the MBOX writer with default save options.
                using (MboxrdStorageWriter writer = new MboxrdStorageWriter(mboxFilePath, new MboxSaveOptions()))
                {
                    // First email message.
                    using (MailMessage message1 = new MailMessage())
                    {
                        message1.From = new MailAddress("alice@example.com", "Alice");
                        message1.To.Add(new MailAddress("bob@example.com", "Bob"));
                        message1.Subject = "Hello Bob";
                        message1.Body = "Hi Bob,\nThis is a test email.\nRegards,\nAlice";

                        writer.WriteMessage(message1);
                    }

                    // Second email message.
                    using (MailMessage message2 = new MailMessage())
                    {
                        message2.From = new MailAddress("carol@example.com", "Carol");
                        message2.To.Add(new MailAddress("dave@example.com", "Dave"));
                        message2.Subject = "Meeting Reminder";
                        message2.Body = "Dear Dave,\nDon't forget our meeting tomorrow at 10 AM.\nBest,\nCarol";

                        writer.WriteMessage(message2);
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"I/O error while writing MBOX file: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}